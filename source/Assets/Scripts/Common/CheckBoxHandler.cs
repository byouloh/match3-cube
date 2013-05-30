
namespace Assets.Scripts.Common
{
	public class CheckBoxHandler : ButtonHandler
	{
		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				SetIsChecked(_isChecked);
			}
		}

		private void SetIsChecked(bool isChecked)
		{
			UICheckbox uiCheckbox = GetComponent<UICheckbox>();
			uiCheckbox.isChecked = isChecked;
		}
	}
}
