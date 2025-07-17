-- Insert sample store data for Toocha Tea
-- Các cửa hàng mẫu tại Hà Nội

INSERT INTO Stores (Name, Address, Latitude, Longitude, PhoneNumber, OpeningHours, IsActive, City, District, Ward, Notes)
VALUES 
-- Hà Nội
('Toocha Tea Hoàn Kiếm', '25 Hàng Bài, Hoàn Kiếm, Hà Nội', 21.0285, 105.8542, '024 3936 1234', '07:00 - 22:00', 1, 'Hà Nội', 'Hoàn Kiếm', 'Hàng Bài', 'Cửa hàng chính - Trung tâm'),

('Toocha Tea Cầu Giấy', '15 Xuân Thủy, Cầu Giấy, Hà Nội', 21.0333, 105.7847, '024 3755 5678', '07:00 - 22:00', 1, 'Hà Nội', 'Cầu Giấy', 'Dịch Vọng Hậu', 'Gần ĐH Quốc Gia'),

('Toocha Tea Ba Đình', '45 Kim Mã, Ba Đình, Hà Nội', 21.0245, 105.8412, '024 3831 9012', '07:00 - 22:00', 1, 'Hà Nội', 'Ba Đình', 'Kim Mã', 'Gần công viên Thống Nhất'),

('Toocha Tea Đống Đa', '88 Láng Hạ, Đống Đa, Hà Nội', 21.0144, 105.8336, '024 3514 3456', '07:00 - 22:00', 1, 'Hà Nội', 'Đống Đa', 'Láng Hạ', 'Khu vực văn phòng'),

('Toocha Tea Hai Bà Trưng', '12 Bà Triệu, Hai Bà Trưng, Hà Nội', 21.0067, 105.8442, '024 3972 7890', '07:00 - 22:00', 1, 'Hà Nội', 'Hai Bà Trưng', 'Bà Triệu', 'Trung tâm thương mại'),

('Toocha Tea Thanh Xuân', '30 Nguyễn Trãi, Thanh Xuân, Hà Nội', 20.9955, 105.8144, '024 3858 1122', '07:00 - 22:00', 1, 'Hà Nội', 'Thanh Xuân', 'Thanh Xuân Nam', 'Khu dân cư đông đúc'),

('Toocha Tea Long Biên', '77 Nguyễn Văn Cừ, Long Biên, Hà Nội', 21.0369, 105.8839, '024 3827 3344', '07:00 - 22:00', 1, 'Hà Nội', 'Long Biên', 'Ngọc Lâm', 'Gần cầu Long Biên'),

-- TP. Hồ Chí Minh
('Toocha Tea Quận 1', '123 Nguyễn Huệ, Quận 1, TP.HCM', 10.8231, 106.6297, '028 3822 5566', '07:00 - 23:00', 1, 'TP. Hồ Chí Minh', 'Quận 1', 'Bến Nghé', 'Phố đi bộ Nguyễn Huệ'),

('Toocha Tea Quận 3', '45 Võ Văn Tần, Quận 3, TP.HCM', 10.7886, 106.6917, '028 3930 7788', '07:00 - 23:00', 1, 'TP. Hồ Chí Minh', 'Quận 3', 'Võ Thị Sáu', 'Trung tâm quận 3'),

('Toocha Tea Bình Thạnh', '88 Điện Biên Phủ, Bình Thạnh, TP.HCM', 10.8019, 106.7054, '028 3844 9900', '07:00 - 23:00', 1, 'TP. Hồ Chí Minh', 'Bình Thạnh', 'Bình Thạnh', 'Gần Landmark 81'),

-- Đà Nẵng
('Toocha Tea Hải Châu', '56 Trần Phú, Hải Châu, Đà Nẵng', 16.0471, 108.2068, '0236 3888 1122', '07:00 - 22:00', 1, 'Đà Nẵng', 'Hải Châu', 'Thạch Thang', 'Trung tâm Đà Nẵng'),

('Toocha Tea Sơn Trà', '12 Hoàng Sa, Sơn Trà, Đà Nẵng', 16.0818, 108.2442, '0236 3999 3344', '07:00 - 22:00', 1, 'Đà Nẵng', 'Sơn Trà', 'Thọ Quang', 'Gần biển Mỹ Khê');

-- Kiểm tra dữ liệu đã insert
SELECT 
    Id,
    Name,
    Address,
    City,
    District,
    CONCAT(Latitude, ', ', Longitude) AS Coordinates,
    PhoneNumber,
    OpeningHours,
    IsActive
FROM Stores
ORDER BY City, District, Name; 