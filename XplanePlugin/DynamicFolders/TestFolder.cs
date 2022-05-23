using System;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class TestFolder : TemplateFolder
    {
        public TestFolder()
        {
            this.DisplayName = "Development Tests";
            this.GroupName = "TEST";
            this.Navigation = Loupedeck.PluginDynamicFolderNavigation.None;
        }

        public override Boolean Activate() {

            SupportClasses.SubscriptionHandler.OnValueChanged += this.SubscriptionHandler_OnValueChanged;

            var sub1 = new TypeClasses.SubscriptionValue();



            return base.Activate();
        }

        private void SubscriptionHandler_OnValueChanged(TypeClasses.SubscriptionValue value)
        {
            this._buttons["test"].caption = value.value.ToString();
            this.CommandImageChanged("test");
        }

        protected override void FillButtons() {
            var temp = new TypeClasses.Button();
            temp.id = "test";
            temp.caption = "test";


            base.FillButtons();
        }

    }
}
