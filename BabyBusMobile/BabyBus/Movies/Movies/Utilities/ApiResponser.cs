namespace BabyBus.Services
{
    /// <summary>
    ///     增删改返回的Api对象
    /// </summary>
    public class ApiResponser
    {        
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Attach { get; set; }
        public int? expires_in { get; set; }
		public override string ToString ()
		{
			return string.Format ("[ApiResponser: Status={0}, Message={1}, Attach={2}, expires_in={3}]", Status, Message, Attach, expires_in);
		}

    }
}