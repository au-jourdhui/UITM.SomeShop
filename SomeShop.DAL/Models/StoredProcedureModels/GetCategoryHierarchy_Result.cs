using System;

namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class GetCategoryHierarchy_Result
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
        public int Level { get; set; }
        public bool IsParent { get; set; }
    }
}
