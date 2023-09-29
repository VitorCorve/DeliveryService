#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using DeliveryService.ApiAssistant;
using DeliveryService.Client.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Client.VIewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            string connectionString = ConfigurationManager.AppSettings["BaseUrl"] ?? throw new InvalidOperationException("Base url isn't defined");
            _client = new ApiAssistant.ApiAssistant(connectionString, new System.Net.Http.HttpClient());
            PackageRequestedDeliveryDate = DateTime.Now;
            InitializeDeliveryStatusesCollection();
        }

        private readonly ApiAssistant.ApiAssistant _client;

        private ObservableCollection<DELIVERY_STATUS> _ticketDeliveryStatus;

        private ObservableCollection<Ticket> _tickets;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Courier> _couriers;
        private ObservableCollection<Package> _packages;

        private DELIVERY_STATUS _selectedTicketDeliveryStatus;

        private Ticket _selectedTicket;
        private Customer _selectedCustomer;
        private Courier _selectedCourier;
        private Package _selectedPackage;

        private string _newCustomerName,
                       _searchQuery,
                       _packageDeliveryAddress,
                       _packageGatheringAddress,
                       _rejectionDescription;

        private bool _canEditTicket,
                     _canEditRejectionDescription;

        private DateTime _packageRequestedDeliveryDate;

        private Command _initialize,
                        _createCustomer,
                        _createCourier,
                        _searchTicket,
                        _loadPackages,
                        _createTicket,
                        _createPackage,
                        _moveToProcessing,
                        _updatePackage,
                        _changeDeliveryStatus,
                        _removeTicket;

        public ObservableCollection<DELIVERY_STATUS> TicketDeliveryStatuses
        {
            get => _ticketDeliveryStatus;
            set => Set(ref _ticketDeliveryStatus, value);
        }

        public ObservableCollection<Ticket> Tickets
        {
            get => _tickets;
            set => Set(ref _tickets, value);
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        public ObservableCollection<Courier> Couriers
        {
            get => _couriers;
            set => Set(ref _couriers, value);
        }
        
        public ObservableCollection<Package> Packages
        {
            get => _packages;
            set => Set(ref _packages, value);
        }

        public Ticket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                Set(ref _selectedTicket, value);
                LoadPackages.Execute(value);
                CheckCanEditTicket();
            }
        }
        
        public DELIVERY_STATUS SelectedTicketDeliveryStatus
        {
            get => _selectedTicketDeliveryStatus;
            set
            {
                Set(ref _selectedTicketDeliveryStatus, value);
                CheckCanEditRejectionDescriptionField();
            }
        }
        
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => Set(ref _selectedCustomer, value);
        }

        public Courier SelectedCourier
        {
            get => _selectedCourier;
            set => Set(ref _selectedCourier, value);
        }
        
        public Package SelectedPackage
        {
            get => _selectedPackage;
            set 
            {
                Set(ref _selectedPackage, value);
                PerformPackageSelection(value);
            }
        }

        public string NewCustomerName
        {
            get => _newCustomerName;
            set => Set(ref _newCustomerName, value);
        }
        
        public string SearchQuery
        {
            get => _searchQuery;
            set => Set(ref _searchQuery, value);
        }
        
        public string PackageDeliveryAddress
        {
            get => _packageDeliveryAddress;
            set => Set(ref _packageDeliveryAddress, value);
        }
        
        public string PackageGatheringAddress
        {
            get => _packageGatheringAddress;
            set => Set(ref _packageGatheringAddress, value);
        }
        
        public string RejectionDescription
        {
            get => _rejectionDescription;
            set => Set(ref _rejectionDescription, value);
        }
        
        public bool CanEditTicket
        {
            get => _canEditTicket;
            set => Set(ref _canEditTicket, value);
        }

        public bool CanEditRejectionDescription
        {
            get => _canEditRejectionDescription;
            set => Set(ref _canEditRejectionDescription, value);
        }
        
        public DateTime PackageRequestedDeliveryDate
        {
            get => _packageRequestedDeliveryDate;
            set => Set(ref _packageRequestedDeliveryDate, value);
        }

        public Command Initialize => _initialize ??= new Command(async obj =>
        {
            await LoadTicketsAsync(null, null, null, null, null, null);
            var customersRequestResult = await _client.CustomersAllAsync();
            var couriersRequestResult = await _client.CouriersAllAsync();

            Customers = new ObservableCollection<Customer>(customersRequestResult);
            Couriers = new ObservableCollection<Courier>(couriersRequestResult);

            SelectedTicket = Tickets.FirstOrDefault();
        });

        public Command CreateCustomer => _createCustomer ??= new Command(async obj =>
        {
            Customer customer = new()
            {
                CustomerName = NewCustomerName,
            };

            await _client.CustomersAsync(customer);

            var customersRequestResult = await _client.CustomersAllAsync();
            Customers = new ObservableCollection<Customer>(customersRequestResult);
            NewCustomerName = string.Empty;
        }, canExecute => !string.IsNullOrWhiteSpace(NewCustomerName));
        
        public Command CreateCourier => _createCourier ??= new Command(async obj =>
        {
            Courier courier = new();
            await _client.CouriersAsync(courier);
            var couriersRequestResult = await _client.CouriersAllAsync();
            Couriers = new ObservableCollection<Courier>(couriersRequestResult);
        });

        public Command SearchTicket => _searchTicket ??= new Command(async obj =>
        {
            await LoadTicketsAsync(null, null, null, null, null, null);
        });

        public Command LoadPackages => _loadPackages ??= new Command(async obj =>
        {
            if (obj != null && obj is Ticket ticket)
            {
                await LoadPackagesAsync(ticket.TicketId);
                SelectedTicketDeliveryStatus = ticket.Status;
                RejectionDescription = string.Empty;
            }

        }, canExecute => SelectedTicket != null);

        public Command CreatePackage => _createPackage ??= new Command(async obj =>
        {
            Package package = new()
            {
                RequestedDeliveryDate = PackageRequestedDeliveryDate,
                DeliveryAddress = PackageDeliveryAddress,
                GatheringAddress = PackageGatheringAddress,
                TicketId = SelectedTicket.TicketId,
            };

            await _client.PackagesAsync(package);
            PackageDeliveryAddress = string.Empty;
            PackageGatheringAddress = string.Empty;
            SelectedCourier = null;

            await LoadPackagesAsync(SelectedTicket.TicketId);

        }, canExecute => 
            !string.IsNullOrWhiteSpace(PackageDeliveryAddress) &&
            !string.IsNullOrWhiteSpace(PackageGatheringAddress) &&
            SelectedTicket != null);

        public Command CreateTicket => _createTicket ??= new Command(async obj =>
        {
            Ticket ticket = await _client.TicketsPOSTAsync(SelectedCustomer.CustomerId);
            var ticketsRequestResult = await _client.TicketsAllAsync(null, null, null, null, searchQuery: SearchQuery, null, null);
            Tickets = new ObservableCollection<Ticket>(ticketsRequestResult);
            SelectedTicket = ticket;
            SelectedCustomer = null;
        }, canExecute => SelectedCustomer != null);

        public Command MoveToProcessing => _moveToProcessing ??= new Command(async obj =>
        {
            int selectedTicketId = SelectedTicket.TicketId;
            await _client.ProccessAsync(selectedTicketId, SelectedCourier.CourierId);
            await LoadTicketsAsync(null, null, null, null, null, null);
            SelectedCourier = null;

            SelectedTicket = Tickets.FirstOrDefault(t => t.TicketId.Equals(selectedTicketId));
        }, canExecute => SelectedCourier != null && 
            SelectedTicket != null && 
            SelectedTicket.Status == DELIVERY_STATUS.New);

        public Command UpdatePackage => _updatePackage ??= new Command(async obj =>
        {
            SelectedPackage.RequestedDeliveryDate = PackageRequestedDeliveryDate;
            SelectedPackage.DeliveryAddress = PackageDeliveryAddress;
            SelectedPackage.GatheringAddress = PackageGatheringAddress;
            SelectedPackage.TicketId = SelectedTicket.TicketId;

            await _client.PackagesPUTAsync(SelectedPackage);
            await LoadPackagesAsync(SelectedTicket.TicketId);
        }, canExecute => 
            !string.IsNullOrWhiteSpace(PackageDeliveryAddress) &&
            !string.IsNullOrWhiteSpace(PackageGatheringAddress) &&
            SelectedTicket != null);
        
        public Command ChangeDeliveryStatus => _changeDeliveryStatus ??= new Command(async obj =>
        {
            await _client.EditAsync(SelectedTicket.TicketId, SelectedTicketDeliveryStatus, RejectionDescription);
            await LoadTicketsAsync(null, null, null, null, null, null);
        }, canExecute => 
            (SelectedTicket != null && SelectedTicketDeliveryStatus != DELIVERY_STATUS.Rejected) ||
            (SelectedTicket != null && !string.IsNullOrEmpty(RejectionDescription)));
        
        
        public Command RemoveTicket => _removeTicket ??= new Command(async obj =>
        {
            await _client.TicketsDELETEAsync(SelectedTicket.TicketId);
            await LoadTicketsAsync(null, null, null, null, null, null);
        }, canExecute => SelectedTicket != null);

        private async Task LoadTicketsAsync(int? customerId, int? courierId, bool? isClosed, DELIVERY_STATUS? status, DateTime? dateFrom, DateTime? dateTo)
        {
            var ticketsRequestResult = await _client.TicketsAllAsync(customerId, courierId, isClosed, status, searchQuery: SearchQuery, dateFrom, dateTo);
            Tickets = new ObservableCollection<Ticket>(ticketsRequestResult);
        }

        private async Task LoadPackagesAsync(int ticketId)
        {
            var packagesRequestResult = await _client.PackagesAllAsync();
            packagesRequestResult = packagesRequestResult.Where(p => p.TicketId.Equals(ticketId)).ToList();
            Packages = new ObservableCollection<Package>(packagesRequestResult);
        }

        private void CheckCanEditTicket() => CanEditTicket = SelectedTicket != null && SelectedTicket.Status == DELIVERY_STATUS.New;

        private void CheckCanEditRejectionDescriptionField() => CanEditRejectionDescription = SelectedTicket != null && SelectedTicketDeliveryStatus == DELIVERY_STATUS.Rejected;

        private void PerformPackageSelection(Package package)
        {
            PackageDeliveryAddress = package is null ? string.Empty : package.DeliveryAddress;
            PackageGatheringAddress = package is null ? string.Empty : package.GatheringAddress;
            PackageRequestedDeliveryDate = package is null ? DateTime.Now : package.RequestedDeliveryDate;
        }

        private void InitializeDeliveryStatusesCollection()
        {
            TicketDeliveryStatuses = new ObservableCollection<DELIVERY_STATUS> 
            { 
                DELIVERY_STATUS.New, 
                DELIVERY_STATUS.Processing, 
                DELIVERY_STATUS.Completed, 
                DELIVERY_STATUS.Rejected 
            };
        }
    }
}

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
