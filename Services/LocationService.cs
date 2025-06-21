using App.Models;
using App.Models.Toocha;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public interface ILocationService
    {
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
        decimal CalculateShippingFee(double distanceKm);
        bool IsDeliverySupported(double distanceKm);
        Task<List<StoreWithDistance>> GetNearestStores(double latitude, double longitude, int limit = 5);
        Task<(double latitude, double longitude)?> GeocodeAddress(string address);
    }

    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;
        private const double BASE_SHIPPING_FEE = 15000; // Phí cơ bản: 15,000đ
        private const double PRICE_PER_KM = 3000; // 3,000đ mỗi km
        private const double FREE_SHIPPING_DISTANCE = 3; // Miễn phí ship trong 3km
        private const double MAX_SHIPPING_DISTANCE = 35; // Khoảng cách giao hàng tối đa: 35km

        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tính khoảng cách giữa 2 điểm bằng công thức Haversine (km)
        /// </summary>
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Bán kính Trái Đất (km)

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return Math.Round(distance, 2);
        }

        /// <summary>
        /// Kiểm tra xem có hỗ trợ giao hàng đến khoảng cách này không
        /// </summary>
        public bool IsDeliverySupported(double distanceKm)
        {
            return distanceKm <= MAX_SHIPPING_DISTANCE;
        }

        /// <summary>
        /// Tính phí giao hàng dựa trên khoảng cách
        /// </summary>
        public decimal CalculateShippingFee(double distanceKm)
        {
            if (!IsDeliverySupported(distanceKm))
            {
                throw new NotSupportedException($"Không hỗ trợ giao hàng cho khoảng cách {distanceKm:F1}km. Khoảng cách tối đa: {MAX_SHIPPING_DISTANCE}km");
            }

            if (distanceKm <= FREE_SHIPPING_DISTANCE)
            {
                return 0; // Miễn phí ship
            }

            var extraDistance = distanceKm - FREE_SHIPPING_DISTANCE;
            var additionalFee = (decimal)(extraDistance * PRICE_PER_KM);
            var fee = (decimal)BASE_SHIPPING_FEE + additionalFee;

            // Làm tròn đến 1000đ
            return Math.Ceiling(fee / 1000) * 1000;
        }

        /// <summary>
        /// Lấy danh sách cửa hàng gần nhất (chỉ trong phạm vi hỗ trợ giao hàng)
        /// </summary>
        public async Task<List<StoreWithDistance>> GetNearestStores(double latitude, double longitude, int limit = 5)
        {
            var stores = await _context.Stores
                .Where(s => s.IsActive)
                .ToListAsync();

            var storesWithDistance = stores.Select(store => {
                var distance = CalculateDistance(latitude, longitude, store.Latitude, store.Longitude);
                return new StoreWithDistance
                {
                    Store = store,
                    Distance = distance,
                    ShippingFee = IsDeliverySupported(distance) ? CalculateShippingFee(distance) : -1, // -1 = không hỗ trợ
                    IsSupported = IsDeliverySupported(distance)
                };
            })
            .Where(s => s.IsSupported) // Chỉ lấy cửa hàng trong phạm vi hỗ trợ
            .OrderBy(s => s.Distance)
            .Take(limit)
            .ToList();

            return storesWithDistance;
        }

        /// <summary>
        /// Chuyển đổi địa chỉ thành tọa độ (Geocoding)
        /// Hiện tại trả về tọa độ mẫu, trong thực tế có thể tích hợp Google Maps API
        /// </summary>
        public async Task<(double latitude, double longitude)?> GeocodeAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return null;

            // TODO: Tích hợp với Google Maps Geocoding API
            // Hiện tại trả về tọa độ trung tâm Hà Nội cho demo
            await Task.Delay(100); // Simulate API call

            // Một số địa chỉ mẫu cho demo
            var sampleLocations = new Dictionary<string, (double, double)>
            {
                ["hà nội"] = (21.0285, 105.8542),
                ["hồ chí minh"] = (10.8231, 106.6297),
                ["đà nẵng"] = (16.0471, 108.2068),
                ["cầu giấy"] = (21.0333, 105.7847),
                ["ba đình"] = (21.0245, 105.8412),
                ["hoàn kiếm"] = (21.0285, 105.8542),
                ["đống đa"] = (21.0144, 105.8336),
                ["hai bà trưng"] = (21.0067, 105.8442)
            };

            address = address.ToLower();
            foreach (var location in sampleLocations)
            {
                if (address.Contains(location.Key))
                {
                    return location.Value;
                }
            }

            // Mặc định trả về tọa độ trung tâm Hà Nội
            return (21.0285, 105.8542);
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

    public class StoreWithDistance
    {
        public Store Store { get; set; }
        public double Distance { get; set; }
        public decimal ShippingFee { get; set; }
        public bool IsSupported { get; set; }
    }
} 