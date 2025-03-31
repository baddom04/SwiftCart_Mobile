using ShoppingList.Core;

namespace ShoppingListEditor.Model.Editables
{
    public class SectionEditable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int MapId { get; set; }
        public static SectionEditable FromSection(Section section)
        {
            return new SectionEditable()
            {
                Id = section.Id,
                Name = section.Name,
                MapId = section.MapId,
            };
        }
    }
}
