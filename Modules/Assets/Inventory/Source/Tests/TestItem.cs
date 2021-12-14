using System.Reflection;

namespace Modules.Inventory.Tests
{
    public class TestItem : ItemBase
    {
        public static TestItem CreateInstance(uint id, string name)
        {
            var result = CreateInstance<TestItem>();

            SetFieldValue(result, "_id", id);
            SetFieldValue(result, "_name", name);

            return result;
        }

        private static void SetFieldValue(TestItem item, string field, object value)
        {
            var fieldInfo = typeof(ItemBase).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(item, value);
        }
    }
}
