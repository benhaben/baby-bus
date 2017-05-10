using System;
using BabyBus.Logic.Shared;
using Newtonsoft.Json;

namespace BabyBus.Logic.Shared
{
	public class ChildInfoViewModel : BaseViewModel
	{
		public ChildInfoViewModel()
		{
		}

		public void Init(string json = "") {
			child = JsonConvert.DeserializeObject<ChildModel>(json);
			ChildName = child.ChildName;
			GenderName = child.GenderName;
			Birthday = child.Birthday;
			ParentName = child.ParentName;
			PhoneNumber = child.PhoneNumber;
			Address = "";
			ImageName = child.ImageName;
			ClassName = BabyBusContext.Class.ClassName;
		}

		#region Property

		private AuditType checkoutAuditType = AuditType.Passed;
		private ChildModel child;
		private byte[] _bytes;

		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		public AuditType CheckoutAuditType {
			get { return checkoutAuditType; }
			private set { checkoutAuditType = value; }
		}

		public string ChildName { get; private set; }

		public string ClassName { get; private set; }

		public string GenderName { get; private set; }

		public DateTime Birthday { get; private set; }

		public string BirthdayString{ get { return Birthday.ToString("D"); } }

		public string ParentName { get; private set; }

		public string PhoneNumber { get; private set; }

		public string Address { get; private set; }

		public string Memo { get; set; }

		public string ImageName { get; set; }

		#endregion
	}
}