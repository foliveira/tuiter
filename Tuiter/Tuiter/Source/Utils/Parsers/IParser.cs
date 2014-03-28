namespace Tuiter.Source.Utils.Parsers
{
    using System.IO;
    using Data;

    internal interface IParser
    {
        void Parse(Stream stream, TuiterDS set);
    }
}