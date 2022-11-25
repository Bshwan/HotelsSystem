
namespace HotelsSystem.Pages.Hotels;
public partial class Hotels
{
    [Inject]
    protected ISqlDataAccess DB { get; set; } = default!;
    [Inject]
    protected IJSRuntime jSRuntime { get; set; } = default!;
    [Inject]
    protected IToaster Toaster { get; set; } = default!;
    [Inject]
    protected NavigationManager nav { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService{get;set;}=default!;


    private void OpenDialog(){
		var options = new DialogOptions {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
        };
        DialogService.Show<AddHotel>("Simple Dialog", options);
    }
}