using System;
using System.Windows;
using System.Windows.Input;
using StreamingPlatform.Helpers;
using StreamingPlatformCore;
using StreamingPlatformCore.Entities;
using StreamingPlatformCore.Services;

namespace StreamingPlatform.Models
{
    public class DonateViewModel : NotifablePropertyChanged
    {
        private readonly int _streamId;
        private readonly ApplicationContext _context;
        public decimal Amount { get; set; }
        public string Message { get; set; } = "";

        public ICommand DonateCommand { get; }
        public ICommand CancelCommand { get; }

        public DonateViewModel(int streamId)
        {
            _streamId = streamId;
            _context = ApplicationContext.GetInstance();
            DonateCommand = new RelayCommand(_ => SendDonate());
            CancelCommand = new RelayCommand(_ => CloseWindow(false));
        }

        private void SendDonate()
        {
            if (Amount <= 0) return;
            var userService = new UserService();
            var donation = new Donation(userService.CurrentUser.Id, _streamId, Amount)
            {
                Message = Message,
                DonationDate = DateTime.UtcNow
            };
            _context.Donates.Add(donation);
            _context.SaveChanges();
            CloseWindow(true);
        }

        private void CloseWindow(bool result)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = result;
                    window.Close();
                    break;
                }
            }
        }
    }
}