using CommunityToolkit.Mvvm.ComponentModel;

namespace YuvaCep.Mobile.Dtos
{
    public partial class StudentListDto : ObservableObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ReferenceCode { get; set; }
        public string ParentName { get; set; }

        [ObservableProperty]
        private bool isSelected;
    }
}