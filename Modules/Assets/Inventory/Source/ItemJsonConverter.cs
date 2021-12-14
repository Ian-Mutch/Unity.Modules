using Newtonsoft.Json;
using System;

namespace Modules.Inventory
{
    public class ItemConverter : JsonConverter<ItemBase>
    {
        public override void WriteJson(JsonWriter writer, ItemBase value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id.ToString());
        }

        public override ItemBase ReadJson(JsonReader reader, Type objectType, ItemBase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            if (value == null)
                throw new InventoryException("Json item conversion error: null item id");

            if (uint.TryParse(value, out var itemId))
                return ItemManager.GetById(itemId);

            throw new InventoryException($"Json item conversion error: invalid uint item id {value}");
        }
    }
}
