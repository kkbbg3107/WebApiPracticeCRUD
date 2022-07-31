namespace WebapiCorePractice.Helper
{
    public interface IDataConvert<TSource, TDestination>
    {
        public TDestination Convert(TSource source);
    }
}
