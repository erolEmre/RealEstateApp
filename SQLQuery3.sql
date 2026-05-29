USE RealEstateApp;

INSERT INTO Houses
(Title, Description, NumberOfRooms, NumberOfBathrooms, Area, Price, ListingDate, IsAvailable, ContactNumber, Address_City, Address_District, Address_Street, Address_BuildingNo, Address_ApartmentNo, EmployeeId, ImageUrl, IsRental)
VALUES
(
    'Kadıköy Merkezde 3+1 Daire',
    'Metro yakını, ebeveyn banyolu, yeni tadilatlı',
    3, 2, 120.5, 8500000.00,
    GETDATE(), 1, '0532 111 22 33',
    'İstanbul', 'Kadıköy', 'Moda Caddesi', '12', '5',
    1, 'https://placehold.co/600x400', 0
),
(
    'Beşiktaş Kiralık 2+1',
    'Deniz manzaralı, eşyalı, merkezi konum',
    2, 1, 85.0, 25000.00,
    GETDATE(), 1, '0533 222 33 44',
    'İstanbul', 'Beşiktaş', 'Barbaros Bulvarı', '45', '3',
    1, 'https://placehold.co/600x400', 1
),
(
    'Çankaya Satılık Villa',
    'Bahçeli, 4 katlı, özel havuzlu',
    5, 3, 320.0, 15000000.00,
    GETDATE(), 1, '0544 333 44 55',
    'Ankara', 'Çankaya', 'Dikmen Caddesi', '7', NULL,
    1, 'https://placehold.co/600x400', 0
);