using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared.Message
{
	public class ImageBytesMessage : MvxMessage
	{
		public ImageBytesMessage(object sender, byte[] bytes)
			: base(sender)
		{
			ImageBytes = bytes;
		}

		public byte[] ImageBytes { get; set; }
	}
}

