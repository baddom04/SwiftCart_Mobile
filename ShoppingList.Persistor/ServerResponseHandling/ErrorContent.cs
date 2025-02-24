namespace ShoppingList.Persistor.ServerResponseHandling
{
    internal class ErrorContent
    {
        public Dictionary<string, List<string>>? FieldErrors { get; set; }

        public string? GeneralError { get; set; }

        public override string? ToString()
        {
            if (FieldErrors == null && GeneralError == null) return null;

            if (FieldErrors == null)
            {
                return GeneralError;
            }
            else
            {
                var res = $"{FieldErrors.First().Value.First()}";
                return res;
            }
        }
    }
}
