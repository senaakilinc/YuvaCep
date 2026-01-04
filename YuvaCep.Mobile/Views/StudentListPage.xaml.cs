using YuvaCep.Mobile.Dtos;
using YuvaCep.Mobile.ViewModels;

namespace YuvaCep.Mobile.Views;

public partial class StudentListPage : ContentPage
{
    public StudentListPage(StudentListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    async void OnStudentSelected(StudentDto student)
    {
        if (student == null) return;
        await Shell.Current.GoToAsync($"{nameof(StudentDetailPage)}?id={student.Id}");
    }
    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StudentListViewModel vm)
        {

            if (!vm.IsBusy)
            {
                vm.LoadStudentsCommand.Execute(null);
            }
        }
    }
}