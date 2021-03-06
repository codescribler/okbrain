﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using Raven.Client;
using Raven.Client.Linq;
using okbrain.Domain.Models;
using okbrain.Extensions;
using okbrain.Models;
using okbrain.Services;

namespace okbrain.Domain.Services
{
    public class PostService
    {
        private readonly IPostSlugDuplicateDetector _postSlugDuplicateDetector;
        private readonly IDocumentSession _session;
        private readonly IDictionary<Guid, PostDto> _posts;
        private readonly ITaxonomy _taxonomy;

        public PostService(IDocumentSession session, IPostSlugDuplicateDetector postSlugDuplicateDetector, ITaxonomy taxonomy)
        {
            _session = session;
            _postSlugDuplicateDetector = postSlugDuplicateDetector;
            _posts = new Dictionary<Guid, PostDto>();
            _taxonomy = taxonomy;
        }

        public void Handles(CreatePost command)
        {
            if(_postSlugDuplicateDetector.Exists(command.Titles[0].ToSlug())) throw new DuplicateSlugException();

            var post = new Post();
            var cmd = new CreatePost(command.AgId, command.Body, command.Teaser, command.EmailTeaser, command.Titles, command.Status, command.PostDate, command.Author,
                                     command.Titles[0].ToSlug());
            post.CreatePost(cmd, _taxonomy);

            var postDto = post.GetDto();
            _session.Store(postDto);
            _session.SaveChanges();

            _posts[postDto.Id] =  postDto;
        }

        public PostDto GetLastPost(Guid id)
        {
            return _posts[id];
        }

        
    }

    public class DuplicateSlugException : Exception
    {
     
    }

    public interface IPostSlugDuplicateDetector
    {
        bool Exists(string slug);
    }

    public class PostSlugDuplicateDetector : IPostSlugDuplicateDetector
    {
        private readonly IDocumentSession _session;
        private int? _maxStaleQueryTries;
        public int MaxStaleQueryRetries
        {
            get { return _maxStaleQueryTries ?? 3; }
            set { _maxStaleQueryTries = value; }
        }

        public PostSlugDuplicateDetector(IDocumentSession session)
        {
            _session = session;
        }

        public bool Exists(string slug)
        {
            return CountedExists(slug, 0);
        }

        private bool CountedExists(string slug, int count)
        {
            RavenQueryStatistics stats;
            IRavenQueryable<PostDto> ravenQueryable = _session.Query<PostDto>().Where(p => p.Slug == slug).Statistics(out stats);

            if (stats.IsStale && count < _maxStaleQueryTries)
            {
                Thread.Sleep(100);
                return CountedExists(slug, count + 1);
            }

            return stats.TotalResults > 0;
        }
    }
}