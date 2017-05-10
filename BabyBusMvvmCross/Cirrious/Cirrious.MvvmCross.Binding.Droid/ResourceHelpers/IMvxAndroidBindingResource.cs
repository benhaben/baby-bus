namespace Cirrious.MvvmCross.Binding.Droid.ResourceHelpers
{
    public interface IMvxAndroidBindingResource
    {
        int BindingTagUnique { get; }
        int[] BindingStylableGroupId { get; }
        int BindingBindId { get; }
        int BindingLangId { get; }
        int[] ControlStylableGroupId { get; }
        int TemplateId { get; }
        int[] ImageViewStylableGroupId { get; }
        int SourceBindId { get; }
        int[] ListViewStylableGroupId { get; }
        int ListItemTemplateId { get; }
        int DropDownListItemTemplateId { get; }

        //Permission Test，随后会把这个切换到自己的项目中
        int[] PermissionStylableGroupId { get; }
        int IsVisiableId { get; }
    }
}