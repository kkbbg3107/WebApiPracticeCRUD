namespace WebapiCorePractice.Helper
{
    /// <summary>
    /// 字串轉類別轉換器
    /// </summary>
    public class TypeDataConvert : ITypeConvert
    {
        public Type Convert(string source)
        {
            if (source is null)
            {
                return null;
            }

            Type type = Type.GetType(source);
            object data = Activator.CreateInstance(type);

            return (Type)data;
        }
    }
}
