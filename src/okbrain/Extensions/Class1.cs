using Nancy;
using Nancy.Responses.Negotiation;


public static class NegotiateExtensions
{
    public static Negotiator ForJson(this Negotiator negotiator, object model)
    {
        return negotiator.WithMediaRangeModel(MediaRange.FromString("application/json"), model);
    }

    public static Negotiator ForXml(this Negotiator negotiator, object model)
    {
        return negotiator.WithMediaRangeModel(MediaRange.FromString("application/xml"), model);
    }

    public static Negotiator OrPartial(this Negotiator negotiator, object model)
    {
        return negotiator.ForJson(model).ForXml(model);
    }
}


