using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

namespace CRM.ViewModel.AdminWindow;

public partial class TaskRequestsPage : Page
{
    public TaskRequestsPage()
    {
        InitializeComponent();
        LoadStatusRequests();
        LoadPendingOrders();
        LoadProfileRequests();
    }

    private void LoadStatusRequests()
    {
        RequestsList.ItemsSource = RequestService.GetPendingStatusRequests();
    }

    private void LoadPendingOrders()
    {
        PendingOrdersList.ItemsSource = TaskService.GetFiltered(
            statusFilters: new List<CRM.Data.Task.TaskStatus> { CRM.Data.Task.TaskStatus.Pending });
    }

    private void LoadProfileRequests()
    {
        ProfileRequestsList.ItemsSource = RequestService.GetPendingProfileRequests();
    }

    private void ApproveBtn(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is not TaskStatusRequest request)
        {
            MessageBox.Show("Select request");
            return;
        }

        if (RequestService.ApproveStatusRequest(request.Id))
        {
            MessageBox.Show("Status request approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadStatusRequests();
        }
    }

    private void RejectBtn(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is not TaskStatusRequest request)
        {
            MessageBox.Show("Select request");
            return;
        }

        if (RequestService.RejectStatusRequest(request.Id))
        {
            MessageBox.Show("Status request rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadStatusRequests();
        }
    }

    private void ApproveOrderBtn(object sender, RoutedEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select order");
            return;
        }

        if (RequestService.ApproveOrder(selectedTask.Id))
        {
            MessageBox.Show("Order approved and set as Available.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadPendingOrders();
        }
    }

    private void DeleteOrderBtn(object sender, RoutedEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select order");
            return;
        }

        var result = MessageBox.Show(
            "Delete this order permanently?",
            "Confirm deletion",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        if (RequestService.DeleteOrder(selectedTask.Id))
        {
            MessageBox.Show("Order deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadPendingOrders();
        }
    }

    private void RequestDetailsBtn(object sender, RoutedEventArgs e)
    {
        if (RequestsList.SelectedItem is TaskStatusRequest request)
            new DetailsWindow(request).ShowDialog();
    }

    private void RequestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (RequestsList.SelectedItem is TaskStatusRequest request)
            new DetailsWindow(request).ShowDialog();
    }

    private void OrderDetailsBtn(object sender, RoutedEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is CRM.Data.Task task)
            new DetailsWindow(task).ShowDialog();
    }

    private void PendingOrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (PendingOrdersList.SelectedItem is CRM.Data.Task task)
            new DetailsWindow(task).ShowDialog();
    }

    private void ApproveProfileBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request)
        {
            MessageBox.Show("Select a request");
            return;
        }

        if (RequestService.ApproveProfileRequest(request.Id))
        {
            MessageBox.Show("Profile changes approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadProfileRequests();
        }
    }

    private void RejectProfileBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is not ProfileChangeRequest request)
        {
            MessageBox.Show("Select a request");
            return;
        }

        if (RequestService.RejectProfileRequest(request.Id))
        {
            MessageBox.Show("Profile changes rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadProfileRequests();
        }
    }

    private void ProfileDetailsBtn(object sender, RoutedEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is ProfileChangeRequest request)
            new DetailsWindow(request).ShowDialog();
    }

    private void ProfileRequestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ProfileRequestsList.SelectedItem is ProfileChangeRequest request)
            new DetailsWindow(request).ShowDialog();
    }
}
