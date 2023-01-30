using ECR.UI;
using RSG;

namespace ECR.Meta.Menu
{
    public class ShopWindow : OneButtonWindow
    {
        protected override void Construct()
        {
            base.Construct();
        }

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            
            return base.InitAndShow(data, titleText);
        }
    }
}