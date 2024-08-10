namespace CatalogService.Api.Core.Domain
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int AvailableStock { get; set; }
        public bool OnReorder { get; set; }

        //Relations
        public int ItemCategoryId { get; set; }
        public int ItemBrandId { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public ItemBrand ItemBrand { get; set; }
    }
}
