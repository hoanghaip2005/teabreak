-- Script để thêm cột Region vào bảng Stores (nếu chưa có)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Stores]') AND name = 'Region')
BEGIN
    ALTER TABLE [Stores] ADD [Region] NVARCHAR(10) NULL;
END
GO

-- Xóa dữ liệu cũ (nếu có) - cẩn thận với foreign key constraints
-- Nếu có Orders tham chiếu, cần xử lý trước
IF EXISTS (SELECT 1 FROM [Orders] WHERE [StoreId] IS NOT NULL)
BEGIN
    PRINT N'Cảnh báo: Có đơn hàng đang tham chiếu đến cửa hàng. Không thể xóa dữ liệu cũ.';
    PRINT N'Sẽ insert thêm dữ liệu mới thay vì thay thế.';
END
ELSE
BEGIN
    DELETE FROM [Stores];
    PRINT N'Đã xóa dữ liệu cửa hàng cũ.';
END

-- Kiểm tra và chỉ insert các cửa hàng chưa có
-- (Tránh duplicate dựa trên tên và địa chỉ)

-- Insert dữ liệu cửa hàng từ HTML tĩnh (chỉ insert nếu chưa có)
-- Sử dụng MERGE để tránh duplicate
MERGE [Stores] AS target
USING (VALUES
-- Miền Bắc (MB)
(N'Tocotoco 66 Phú Mỹ', N'66 Phú Mỹ, Nam Từ Liêm, Hà Nội', 21.03050156908905, 105.77123693248919, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 31 Ngõ 8 Lê Quang Đạo', N'Số 31 Ngõ 8 Lê Quang Đạo, Nam Từ Liêm, Hà Nội', 21.0145998, 105.7681232, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 35 Phú Đô', N'35 Phú Đô, Mễ Trì, Nam Từ Liêm, Hà Nội', 21.0081, 105.766, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 29 Nguyễn An Ninh', N'29 Nguyễn An Ninh, Tương Mai, Hoàng Mai, Hà Nội', 20.9916, 105.846, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 109 E5 Tạ Quang Bửu', N'109 E5 Tạ Quang Bửu, Bách Khoa, Hai Bà Trưng, Hà Nội', 21.0023, 105.849, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 1A An Trường', N'1A Đ. An Trường, An Khánh, Hoài Đức, Hà Nội', 20.992, 105.73, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 12 Ngõ 14 Mễ Trì Hạ', N'Số 12 Ngõ 14 Mễ Trì Hạ, Nam Từ Liêm, Hà Nội', 21.01420021057129, 105.77999877929688, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 79 La Nội', N'79 La Nội, Dương Kinh, Hà Đông, Hà Nội', 20.981000900268555, 105.7490005493164, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 310 Phương Canh', N'Số 12 Ngõ 14 Mễ Trì Hạ, Nam Từ Liêm, Hà Nội', 21.03459930419922, 105.74199676513672, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 215 Giáp Nhất', N'215 P. Giáp Nhất, Nhân Chính, Thanh Xuân, Hà Nội', 21.007600784301758, 105.81500244140625, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 43 Ngõ 108 Trần Phú', N'Số 43 Ngõ 108 Trần Phú, Mộ Lao, Hà Đông, Hà Nội', 20.979400634765625, 105.78299713134766, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco Park 8 Times City', N'Park 8 - Times City Park Hill Premium, Hai Bà Trưng, Hà Nội', 20.9918391, 105.8656473, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 23 Tô Ngọc Vân', N'23 Tô Ngọc Vân, Quảng An, Tây Hồ, Hà Nội', 21.068500518798828, 105.82499694824219, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 42 Lò Đúc', N'42 Lò Đúc, Hai Bà Trưng, Hà Nội', 21.01689910888672, 105.85600280761719, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 5 Ngô Thì Nhậm', N'5 Ngô Thì Nhậm, Hà Cầu, Hà Đông, Hà Nội', 20.967300415039062, 105.7699966430664, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 66 Tây Tựu', N'66 Tây Tựu, Bắc Từ Liêm, Hà Nội', 21.074600219726562, 105.72799682617188, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 154 Mỹ Đình', N'154 Đ. Mỹ Đình, Nam Từ Liêm, Hà Nội', 21.028200149536133, 105.7750015258789, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 114 Mễ Trì Thượng', N'114 Mễ Trì Thượng, Nam Từ Liêm, Hà Nội', 21.006500244140625, 105.77799987792969, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco IEC Thanh Trì', N'Chung cư IEC Residences, Đường Trần Thủ Độ, Tứ Hiệp, Thanh Trì, Hà Nội', 20.940799713134766, 105.85900115966797, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 61 Gia Thượng', N'61 Đ. Gia Thượng, Ngọc Thụy, Long Biên, Hà Nội', 21.06100082397461, 105.86799621582031, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 45 Ỷ La', N'45 Đường Ỷ La, Hà Đông, Hà Nội', 20.976999282836914, 105.74600219726562, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 36 Nguyên Xá', N'36 Nguyên Xá, Minh Khai, Bắc Từ Liêm, Hà Nội', 21.05340003967285, 105.73899841308594, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 115 Ngõ 254 Minh Khai', N'Số 115 Ngõ 254 Minh Khai, Mai Động, Hoàng Mai, Hà Nội', 20.991199493408203, 105.85900115966797, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 58 Hương Viên', N'58 P. Hương Viên, Đồng Nhân, Hai Bà Trưng, Hà Nội', 21.01099967956543, 105.8499984741211, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco GO! Long Biên', N'Savico Megamall, 7-9 Đ. Nguyễn Văn Linh, Gia Thụy, Long Biên, Hà Nội', 21.05069923400879, 105.89299774169922, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 41 Đại Đồng', N'41 P. Đại Đồng, Thanh Trì, Hoàng Mai, Hà Nội', 20.997299194335938, 105.88300323486328, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 8 LK3 Văn Khê', N'Số 8 Liền kề 3 KĐT Văn Khê, Hà Đông, Hà Nội', 20.972400665283203, 105.76200103759766, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 62 Nguyễn Hoàng Tôn', N'62 Đ. Nguyễn Hoàng Tôn, Xuân La, Tây Hồ, Hà Nội', 21.071800231933594, 105.80599975585938, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 82 Vân Canh', N'82 Vân Canh, Hoài Đức, Hà Nội', 21.035200119018555, 105.73699951171875, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 71 Ngô Đình Mẫn', N'71 P. Ngô Đình Mẫn, La Khê, Hà Đông, Hà Nội', 20.969100952148438, 105.76100158691406, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 153 Cầu Diễn', N'153 Đ. Cầu Diễn, Phúc Diễn, Bắc Từ Liêm, Hà Nội', 21.045000076293945, 105.75, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 132 Phúc Tân', N'132 Phúc Tân, Hoàn Kiếm, Hà Nội', 21.03689956665039, 105.8550033569336, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco Ngõ 16 Tả Thanh Oai', N'Ngõ 16 Tả Thanh Oai, Thanh Trì, Hà Nội', 20.947200775146484, 105.80899810791016, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 72 Nguyễn Đổng Chi', N'72 Nguyễn Đổng Chi, Cầu Diễn, Nam Từ Liêm, Hà Nội', 21.038700103759766, 105.76399993896484, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 91 Tổ 12 Phú Lương', N'91 Tổ 12 Phú Lương, Hà Đông, Hà Nội', 20.937400817871094, 105.76000213623047, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco TT5 N7 Bắc Linh Đàm', N'N7 TT5 KĐT Bắc Linh Đàm, P. Đại Kim, Quận Hoàng Mai, Hà Nội', 20.971799850463867, 105.83000183105469, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 50 Hoàng Văn Thái', N'50 P. Hoàng Văn Thái, Khương Mai, Thanh Xuân, Hà Nội', 20.996700286865234, 105.8280029296875, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 123 Nam Dư', N'123 P. Nam Dư, Lĩnh Nam, Hoàng Mai, Hà Nội', 20.984800338745117, 105.88800048828125, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 46 Bạch Đằng, Hai Bà Trưng', N'46 Đ. Bạch Đằng, Thanh Lương, Hai Bà Trưng, Hà Nội', 21.00909996032715, 105.86799621582031, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 625 Bạch Đằng, Hoàn Kiếm', N'625 Đ. Bạch Đằng, Chương Dương, Hoàn Kiếm, Hà Nội', 21.0216007232666, 105.86299896240234, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 131 Tựu Liệt', N'131 Đ. Tựu Liệt, Tam Hiệp, Thanh Trì, Hà Nội', 20.953399658203125, 105.83599853515625, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 104 Thanh Bình', N'104 Đ. Thanh Bình, P. Mộ Lao, Hà Đông, Hà Nội', 20.979774475097656, 105.78002166748047, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 31 Hàng Mã', N'31 P. Hàng Mã, Hoàn Kiếm, Hà Nội', 21.03691291809082, 105.84857177734375, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco LK26 Văn Phú', N'LK26 Khu đô thị Văn Phú, Phú La, Hà Đông, Hà Nội', 20.95789909362793, 105.76499938964844, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),
(N'Tocotoco 100 Phùng Hưng', N'100 Đ. Phùng Hưng, P. Phúc La, Hà Đông, Hà Nội', 20.9685001373291, 105.78600311279297, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MB'),

-- Miền Nam (MN)
(N'Tocotoco 421 Đỗ Xuân Hợp', N'421 Đỗ Xuân Hợp, Phước Long B, Quận 9, Thành phố Hồ Chí Minh', 10.819, 106.773, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Toco Vạn Hạnh Mall', N'11 Su Van Hạnh - Van Hạnh Mall', 10.770000457763672, 106.66899871826172, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 30 Quốc lộ 13 cũ', N'30 QL13, Hiệp Bình Chánh, Thủ Đức, Thành phố Hồ Chí Minh', 10.857999801635742, 106.72200012207031, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Toco 48/2 Bis Nguyễn Ảnh Thủ Q.12', N'48/2 Bis Nguyễn Ảnh Thủ, Q.12, TPHCM', 10.873600006103516, 106.6218032836914, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 195 Cao Đạt', N'195 Đ. Cao Đạt, Phường 1, Quận 5, Thành phố Hồ Chí Minh', 10.754300117492676, 106.68299865722656, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 9/18 Đặng Thúc Vịnh', N'9/18 Đ. Đặng Thúc Vịnh, Thới Tam Thôn, Hóc Môn, Thành phố Hồ Chí Minh', 10.902999877929688, 106.62999725341797, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 156 Âu Cơ', N'156 Đ. Âu Cơ, Phường 9, Tân Bình, Thành phố Hồ Chí Minh', 10.771599769592285, 106.6500015258789, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 50/13 Lý Thường Kiệt', N'50/13 Lý Thường Kiệt, TT. Hóc Môn, Hóc Môn, Thành phố Hồ Chí Minh', 10.886799812316895, 106.58899688720703, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 62 Phổ Quang', N'62 Phổ Quang, Phường 2, Tân Bình, Thành phố Hồ Chí Minh', 10.805000305175781, 106.66699981689453, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 88C Đường số 2', N'88C Đường Số 2, Trường Thọ, Thủ Đức, Thành phố Hồ Chí Minh', 10.836400032043457, 106.75399780273438, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 97 Đường số 7', N'97 Đường Số 7, Bình Tân, Thành phố Hồ Chí Minh', 10.80413818359375, 106.60271453857422, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 578 Tân Kỳ Tân Quý', N'578 Đ. Tân Kỳ Tân Quý, Bình Tân, Thành phố Hồ Chí Minh', 10.794612884521484, 106.60942840576172, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 73 Tô Hiệu', N'73 Tô Hiệu, Phú Thạnh, Tân Phú, Thành phố Hồ Chí Minh', 10.775490760803223, 106.62786102294922, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 4 Lê Bình', N'4 Lê Bình, Phường 4, Tân Bình, Thành phố Hồ Chí Minh', 10.794079780578613, 106.65601348876953, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 200 Nguyễn Thị Tú', N'200 Nguyễn Thị Tú, Bình Hưng Hoà B, Bình Tân, Thành phố Hồ Chí Minh', 10.81559944152832, 106.58991241455078, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 152 Nguyễn Phúc Nguyên', N'152 Nguyễn Phúc Nguyên, Phường 10, Quận 3, Thành phố Hồ Chí Minh', 10.780699729919434, 106.67900085449219, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN'),
(N'Tocotoco 278 Tôn Đản', N'278 Tôn Đản, Phường 4, Quận 4, Thành phố Hồ Chí Minh', 10.756799697875977, 106.70600128173828, N'1900.63.69.36', N'9 giờ 00 - 22 giờ 00 (kể cả CN và ngày lễ)', 1, N'MN')
) AS source ([Name], [Address], [Latitude], [Longitude], [PhoneNumber], [OpeningHours], [IsActive], [Region])
ON target.[Name] = source.[Name] AND target.[Address] = source.[Address]
WHEN MATCHED THEN
    UPDATE SET 
        [Latitude] = source.[Latitude],
        [Longitude] = source.[Longitude],
        [PhoneNumber] = source.[PhoneNumber],
        [OpeningHours] = source.[OpeningHours],
        [IsActive] = source.[IsActive],
        [Region] = source.[Region]
WHEN NOT MATCHED THEN
    INSERT ([Name], [Address], [Latitude], [Longitude], [PhoneNumber], [OpeningHours], [IsActive], [Region])
    VALUES (source.[Name], source.[Address], source.[Latitude], source.[Longitude], source.[PhoneNumber], source.[OpeningHours], source.[IsActive], source.[Region]);

-- Cập nhật thông tin thành phố, quận/huyện từ địa chỉ
UPDATE [Stores] SET 
    [City] = CASE 
        WHEN [Region] = 'MB' THEN N'Hà Nội'
        WHEN [Region] = 'MN' THEN N'Thành phố Hồ Chí Minh'
    END;

-- Thêm thông tin quận/huyện dựa trên địa chỉ
UPDATE [Stores] SET [District] = 
    CASE 
        WHEN [Address] LIKE N'%Nam Từ Liêm%' THEN N'Nam Từ Liêm'
        WHEN [Address] LIKE N'%Bắc Từ Liêm%' THEN N'Bắc Từ Liêm'
        WHEN [Address] LIKE N'%Hoàng Mai%' THEN N'Hoàng Mai'
        WHEN [Address] LIKE N'%Hai Bà Trưng%' THEN N'Hai Bà Trưng'
        WHEN [Address] LIKE N'%Hà Đông%' THEN N'Hà Đông'
        WHEN [Address] LIKE N'%Thanh Xuân%' THEN N'Thanh Xuân'
        WHEN [Address] LIKE N'%Hoàn Kiếm%' THEN N'Hoàn Kiếm'
        WHEN [Address] LIKE N'%Tây Hồ%' THEN N'Tây Hồ'
        WHEN [Address] LIKE N'%Long Biên%' THEN N'Long Biên'
        WHEN [Address] LIKE N'%Thanh Trì%' THEN N'Thanh Trì'
        WHEN [Address] LIKE N'%Hoài Đức%' THEN N'Hoài Đức'
        WHEN [Address] LIKE N'%Quận 9%' THEN N'Quận 9'
        WHEN [Address] LIKE N'%Quận 12%' THEN N'Quận 12'
        WHEN [Address] LIKE N'%Quận 5%' THEN N'Quận 5'
        WHEN [Address] LIKE N'%Quận 3%' THEN N'Quận 3'
        WHEN [Address] LIKE N'%Quận 4%' THEN N'Quận 4'
        WHEN [Address] LIKE N'%Tân Bình%' THEN N'Tân Bình'
        WHEN [Address] LIKE N'%Tân Phú%' THEN N'Tân Phú'
        WHEN [Address] LIKE N'%Bình Tân%' THEN N'Bình Tân'
        WHEN [Address] LIKE N'%Thủ Đức%' THEN N'Thủ Đức'
        WHEN [Address] LIKE N'%Hóc Môn%' THEN N'Hóc Môn'
    END
WHERE [District] IS NULL;

PRINT N'Đã insert thành công dữ liệu cửa hàng!';

-- Hiển thị thống kê
DECLARE @TotalStores INT, @NorthStores INT, @SouthStores INT;
SELECT @TotalStores = COUNT(*) FROM [Stores];
SELECT @NorthStores = COUNT(*) FROM [Stores] WHERE [Region] = 'MB';
SELECT @SouthStores = COUNT(*) FROM [Stores] WHERE [Region] = 'MN';

PRINT N'Tổng số cửa hàng: ' + CAST(@TotalStores AS NVARCHAR(10));
PRINT N'Cửa hàng Miền Bắc: ' + CAST(@NorthStores AS NVARCHAR(10));
PRINT N'Cửa hàng Miền Nam: ' + CAST(@SouthStores AS NVARCHAR(10)); 