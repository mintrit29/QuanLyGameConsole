using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;

namespace QuanLyGameConsole
{
    public class SeedData
    {
        public static async Task SeedingData(GameConsoleContext _context)
        {
            await _context.Database.MigrateAsync();
            if (!_context.Brands.Any())
            {
                Brand Nintendo = new Brand { Name = "Nintendo", Slug = "nintendo" };
                Brand Playstation = new Brand { Name = "Playstation", Slug = "playstation" };
                Brand Xbox = new Brand { Name = "Xbox", Slug = "xbox" };
                Brand Sega = new Brand { Name = "Sega", Slug = "sega" };

                await _context.Brands.AddRangeAsync(Nintendo, Playstation, Xbox, Sega);
                await _context.SaveChangesAsync();

            }
            if (!_context.Categories.Any())
            {

                Category PSP = new Category { CategoryName = "PlayStation Portable", ParentId = null, Slug = "playstation-portable" };
                Category PS = new Category { CategoryName = "PlayStation", ParentId = null, Slug = "playstation" };
                Category NintendoSwitch = new Category { CategoryName = "Nintendo Switch", ParentId = null, Slug = "nintendo-switch" };
                Category Nintendo64 = new Category { CategoryName = "Nintendo 64", ParentId = null, Slug = "nintendo-64" };
                Category XboxOne = new Category { CategoryName = "Xbox One", ParentId = null, Slug = "xbox-one" };
                Category Xbox360 = new Category { CategoryName = "Xbox 360", ParentId = null, Slug = "xbox-360" };
                Category Dreamcast = new Category { CategoryName = "Sega Dreamcast", ParentId = null, Slug = "sega-dreamcast" };
                Category GameGear = new Category { CategoryName = "Sega Game Gear", ParentId = null, Slug = "sega-game-gear" };



                await _context.Categories.AddRangeAsync(PSP, PS, NintendoSwitch, Nintendo64, XboxOne, Xbox360, Dreamcast, GameGear);
                await _context.SaveChangesAsync();
            }

            if (!_context.Products.Any())
            {
                // Categories (console types)
                var playstation = _context.Categories.FirstOrDefault(c => c.CategoryName == "PlayStation");
                var psp = _context.Categories.FirstOrDefault(c => c.CategoryName == "PlayStation Portable");
                var nintendoSwitch = _context.Categories.FirstOrDefault(c => c.CategoryName == "Nintendo Switch");
                var nintendo64 = _context.Categories.FirstOrDefault(c => c.CategoryName == "Nintendo 64");
                var xboxOne = _context.Categories.FirstOrDefault(c => c.CategoryName == "Xbox One");
                var xbox360 = _context.Categories.FirstOrDefault(c => c.CategoryName == "Xbox 360");
                var dreamcast = _context.Categories.FirstOrDefault(c => c.CategoryName == "Sega Dreamcast");
                var gameGear = _context.Categories.FirstOrDefault(c => c.CategoryName == "Sega Game Gear");

                // Brands (console brands)
                var nintendo = _context.Brands.FirstOrDefault(b => b.Name == "Nintendo");
                var playstationBrand = _context.Brands.FirstOrDefault(b => b.Name == "Playstation");
                var xbox = _context.Brands.FirstOrDefault(b => b.Name == "Xbox");
                var sega = _context.Brands.FirstOrDefault(b => b.Name == "Sega");


                await _context.Products.AddRangeAsync(
                    new Product
                    {
                        Image = "Nintendo-Switch.png",
                        ProductName = "Nintendo Switch",
                        CategoryId = nintendoSwitch?.CategoryId,
                        BrandId = nintendo?.BrandId,
                        Price = 7590000,
                        ShortDescription = "Máy chơi game đa năng, có thể chơi ở chế độ cầm tay hoặc xuất lên TV.",
                        Description = "Nintendo Switch là một máy chơi game đột phá của Nintendo, cho phép bạn chơi game mọi lúc, mọi nơi. Với thiết kế độc đáo, bạn có thể sử dụng nó như một máy chơi game cầm tay với màn hình 6.2 inch, hoặc kết nối với TV để có trải nghiệm chơi game trên màn hình lớn. Tay cầm Joy-Con có thể tháo rời mang lại nhiều cách chơi sáng tạo.",
                        Specification = "<b>Màn hình:</b> 6.2 inch LCD, 1280x720 pixels<br><b>CPU/GPU:</b> NVIDIA Custom Tegra processor<br><b>Bộ nhớ trong:</b> 32 GB (có thể mở rộng bằng thẻ microSD)<br><b>Thời lượng pin:</b> Khoảng 4.5 - 9 tiếng<br><b>Kết nối:</b> Wi-Fi, Bluetooth 4.1<br><b>Chế độ chơi:</b> TV mode, Tabletop mode, Handheld mode",
                        Quantity = 20,
                        Status = 1,
                        Views = 2500,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "nintendo-switch"
                    },
                    new Product
                    {
                        Image = "Nintendo-64.png",
                        ProductName = "Nintendo 64",
                        CategoryId = nintendo64?.CategoryId,
                        BrandId = nintendo?.BrandId,
                        Price = 2500000,
                        ShortDescription = "Hệ máy chơi game 64-bit huyền thoại của Nintendo, mở ra kỷ nguyên đồ họa 3D.",
                        Description = "Nintendo 64, thường được gọi là N64, là một trong những hệ máy console có ảnh hưởng nhất trong lịch sử. Đây là máy chơi game đầu tiên của Nintendo có đồ họa 3D thực sự, mang đến những trải nghiệm không thể quên với các tựa game kinh điển như Super Mario 64 và The Legend of Zelda: Ocarina of Time. Tay cầm độc đáo với cần analog đã định hình cách chúng ta chơi game ngày nay.",
                        Specification = "<b>CPU:</b> 64-bit NEC VR4300 @ 93.75 MHz<br><b>GPU:</b> SGI RCP @ 62.5 MHz<br><b>RAM:</b> 4 MB RDRAM (có thể nâng cấp lên 8 MB)<br><b>Đồ họa:</b> 256x224 đến 640x480 pixels<br><b>Âm thanh:</b> Stereo 16-bit, 48 kHz<br><b>Phương tiện:</b> Băng game (Cartridge)",
                        Quantity = 15,
                        Status = 1,
                        Views = 1800,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "nintendo-64"
                    },
                    new Product
                    {
                        Image = "Playstation-1.png",
                        ProductName = "PlayStation",
                        CategoryId = playstation?.CategoryId,
                        BrandId = playstationBrand?.BrandId,
                        Price = 1760000,
                        ShortDescription = "Hệ máy console mang tính biểu tượng của Sony, khởi đầu cho một đế chế game.",
                        Description = "PlayStation (PS1) đã thay đổi ngành công nghiệp game mãi mãi với việc sử dụng định dạng đĩa CD, mang lại đồ họa 3D ấn tượng và âm thanh chất lượng cao. Với một thư viện game khổng lồ và đa dạng, từ Final Fantasy VII đến Metal Gear Solid, PS1 đã trở thành một phần không thể thiếu trong tuổi thơ của nhiều thế hệ game thủ.",
                        Specification = "<b>CPU:</b> 32-bit RISC @ 33.9 MHz<br><b>RAM:</b> 2 MB Main, 1 MB Video<br><b>Đồ họa:</b> 16.7 triệu màu, độ phân giải lên tới 640x480<br><b>Âm thanh:</b> 16-bit, 24 kênh ADPCM<br><b>Phương tiện:</b> CD-ROM",
                        Quantity = 18,
                        Status = 1,
                        Views = 2200,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "playstation"
                    },
                    new Product
                    {
                        Image = "Playstation-Portable.png",
                        ProductName = "PlayStation Portable",
                        CategoryId = psp?.CategoryId,
                        BrandId = playstationBrand?.BrandId,
                        Price = 1900000,
                        ShortDescription = "Máy chơi game cầm tay mạnh mẽ của Sony với khả năng đa phương tiện.",
                        Description = "PlayStation Portable (PSP) không chỉ là một chiếc máy chơi game cầm tay, mà còn là một trung tâm giải trí di động. Với màn hình rộng sắc nét, PSP cho phép bạn chơi game, xem phim, nghe nhạc và lướt web. Thư viện game đa dạng với các tựa game nổi tiếng như God of War và Grand Theft Auto đã làm nên thành công của hệ máy này.",
                        Specification = "<b>Màn hình:</b> 4.3 inch TFT LCD, 480x272 pixels<br><b>CPU:</b> MIPS R4000-based @ 20-333 MHz<br><b>RAM:</b> 32 MB (PSP-1000), 64 MB (các đời sau)<br><b>Lưu trữ:</b> Memory Stick Duo/PRO Duo<br><b>Phương tiện:</b> Universal Media Disc (UMD)<br><b>Kết nối:</b> Wi-Fi (trừ PSP-E1000), USB",
                        Quantity = 25,
                        Status = 1,
                        Views = 3000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "playstation-portable"
                    },
                    new Product
                    {
                        Image = "Xbox-One.png",
                        ProductName = "Xbox One",
                        CategoryId = xboxOne?.CategoryId,
                        BrandId = xbox?.BrandId,
                        Price = 4600000,
                        ShortDescription = "Hệ thống giải trí tất cả trong một của Microsoft, kết hợp game, TV và ứng dụng.",
                        Description = "Xbox One được Microsoft định vị là một hệ thống giải trí toàn diện cho phòng khách. Không chỉ chơi các tựa game độc quyền hấp dẫn như Halo và Forza, Xbox One còn tích hợp truyền hình, các dịch vụ streaming và nhiều ứng dụng khác. Tay cầm Xbox One được đánh giá cao về thiết kế công thái học, mang lại sự thoải mái khi chơi trong thời gian dài.",
                        Specification = "<b>CPU:</b> 8-core AMD 'Jaguar' processor @ 1.75 GHz<br><b>GPU:</b> AMD Radeon GCN<br><b>RAM:</b> 8 GB DDR3<br><b>Ổ cứng:</b> 500 GB / 1 TB<br><b>Đồ họa:</b> Hỗ trợ độ phân giải 4K (cho video và một số game)<br><b>Âm thanh:</b> Dolby Digital 5.1, DTS 5.1",
                        Quantity = 12,
                        Status = 1,
                        Views = 1500,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "xbox-one"
                    },
                    new Product
                    {
                        Image = "Xbox-360.png",
                        ProductName = "Xbox 360",
                        CategoryId = xbox360?.CategoryId,
                        BrandId = xbox?.BrandId,
                        Price = 2200000,
                        ShortDescription = "Thế hệ console thứ hai của Microsoft, nổi tiếng với dịch vụ Xbox Live và đồ họa HD.",
                        Description = "Xbox 360 là một bước tiến lớn so với thế hệ Xbox đầu tiên, mang đến đồ họa độ nét cao (HD) và một hệ sinh thái trực tuyến mạnh mẽ với Xbox Live. Dòng máy này sở hữu một thư viện game đồ sộ với nhiều series nổi tiếng như Gears of War, Halo 3, và Mass Effect. Phụ kiện Kinect cũng mang đến một phương thức chơi game mới lạ dựa trên chuyển động.",
                        Specification = "<b>CPU:</b> 3.2 GHz PowerPC Tri-Core Xenon<br><b>GPU:</b> ATI Xenos @ 500 MHz<br><b>RAM:</b> 512 MB GDDR3<br><b>Lưu trữ:</b> Ổ cứng 20/60/120/250/500 GB<br><b>Đồ họa:</b> Hỗ trợ 720p, 1080i, 1080p<br><b>Kết nối:</b> Ethernet, hỗ trợ Wi-Fi (ở các phiên bản sau)",
                        Quantity = 10,
                        Status = 1,
                        Views = 1200,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "xbox-360"
                    },
                    new Product
                    {
                        Image = "Sega-Dreamcast.png",
                        ProductName = "Sega Dreamcast",
                        CategoryId = dreamcast?.CategoryId,
                        BrandId = sega?.BrandId,
                        Price = 3000000,
                        ShortDescription = "Chiếc console cuối cùng của Sega, một huyền thoại đi trước thời đại với khả năng online.",
                        Description = "Dù có vòng đời ngắn ngủi, Sega Dreamcast vẫn được nhớ đến như một hệ máy sáng tạo và đi trước thời đại. Đây là console đầu tiên tích hợp sẵn modem để chơi game online. Dreamcast sở hữu những tựa game độc đáo và được đánh giá cao như Shenmue, Sonic Adventure, và Crazy Taxi, để lại một di sản bền vững trong lòng người hâm mộ.",
                        Specification = "<b>CPU:</b> Hitachi SH-4 32-bit RISC @ 200 MHz<br><b>GPU:</b> NEC PowerVR2<br><b>RAM:</b> 16 MB Main, 8 MB Video, 2 MB Sound<br><b>Phương tiện:</b> GD-ROM<br><b>Kết nối:</b> Modem 56K tích hợp (có thể nâng cấp lên băng thông rộng)<br><b>Tính năng đặc biệt:</b> Visual Memory Unit (VMU)",
                        Quantity = 8,
                        Status = 2,
                        Views = 900,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "sega-dreamcast"
                    },
                    new Product
                    {
                        Image = "Sega-Game-Gear.png",
                        ProductName = "Sega Game Gear",
                        CategoryId = gameGear?.CategoryId,
                        BrandId = sega?.BrandId,
                        Price = 1500000,
                        ShortDescription = "Máy chơi game cầm tay 8-bit của Sega với màn hình màu đầy ấn tượng.",
                        Description = "Ra mắt để cạnh tranh với Game Boy của Nintendo, Sega Game Gear nổi bật với màn hình LCD màu có đèn nền, một tính năng cao cấp vào thời điểm đó. Máy có thể chơi các tựa game của hệ máy Master System thông qua một bộ chuyển đổi. Dù có thời lượng pin ngắn, Game Gear vẫn được yêu thích nhờ thư viện game chất lượng và trải nghiệm hình ảnh vượt trội.",
                        Specification = "<b>CPU:</b> Zilog Z80 @ 3.5 MHz<br><b>RAM:</b> 8 KB RAM, 16 KB VRAM<br><b>Màn hình:</b> 3.2 inch backlit LCD, 160x144 pixels<br><b>Bảng màu:</b> 4096 màu, 32 màu trên màn hình<br><b>Âm thanh:</b> Mono speaker, jack cắm tai nghe<br><b>Nguồn:</b> 6 viên pin AA",
                        Quantity = 14,
                        Status = 2,
                        Views = 750,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null,
                        Deleted = 0,
                        Slug = "sega-game-gear"
                    }
                );
                await _context.SaveChangesAsync();

                await _context.ProductImages.AddRangeAsync(
                    // Nintendo Switch Images (ProductId = 1)
                    new ProductImage { ProductId = 1, Image = "Nintendo-Switch.png" },
                    new ProductImage { ProductId = 1, Image = "Nintendo-Switch-2.png" },
                    new ProductImage { ProductId = 1, Image = "Nintendo-Switch-3.png" },

                    // Nintendo 64 Images (ProductId = 2)
                    new ProductImage { ProductId = 2, Image = "Nintendo-64.png" },
                    new ProductImage { ProductId = 2, Image = "Nintendo-64-2.png" },
                    new ProductImage { ProductId = 2, Image = "Nintendo-64-3.png" },

                    // PlayStation Images (ProductId = 3)
                    new ProductImage { ProductId = 3, Image = "Playstation-1.png" },
                    new ProductImage { ProductId = 3, Image = "Playstation-1-2.png" },
                    new ProductImage { ProductId = 3, Image = "Playstation-1-3.png" },

                    // PlayStation Portable Images (ProductId = 4)
                    new ProductImage { ProductId = 4, Image = "Playstation-Portable.png" },
                    new ProductImage { ProductId = 4, Image = "Playstation-Portable-2.png" },
                    new ProductImage { ProductId = 4, Image = "Playstation-Portable-3.png" },

                    // Xbox One Images (ProductId = 5)
                    new ProductImage { ProductId = 5, Image = "Xbox-One.png" },
                    new ProductImage { ProductId = 5, Image = "Xbox-One-2.png" },
                    new ProductImage { ProductId = 5, Image = "Xbox-One-3.png" },

                    // Xbox 360 Images (ProductId = 6)
                    new ProductImage { ProductId = 6, Image = "Xbox-360.png" },
                    new ProductImage { ProductId = 6, Image = "Xbox-360-2.png" },
                    new ProductImage { ProductId = 6, Image = "Xbox-360-3.png" },

                    // Sega Dreamcast Images (ProductId = 7)
                    new ProductImage { ProductId = 7, Image = "Sega-Dreamcast.png" },
                    new ProductImage { ProductId = 7, Image = "Sega-Dreamcast-2.png" },
                    new ProductImage { ProductId = 7, Image = "Sega-Dreamcast-3.png" },

                    // Sega Game Gear Images (ProductId = 8)
                    new ProductImage { ProductId = 8, Image = "Sega-Game-Gear.png" },
                    new ProductImage { ProductId = 8, Image = "Sega-Game-Gear-2.png" },
                    new ProductImage { ProductId = 8, Image = "Sega-Game-Gear-3.png" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Footers.Any())
            {
                await _context.Footers.AddRangeAsync(
                    new Footer
                    {
                        Logo = "Logo-Game-Store.png",
                        Description = "GameZ không chỉ là nơi để mua sắm máy và đĩa game, mà còn là một nơi để khám phá, giao lưu và đắm mình trong thế giới game đầy màu sắc.",
                        Address = "387 Đường Nguyễn Công Trứ, phường Cầu Ông Lãnh, Quận 1, TP. Hồ Chí Minh",
                        Email = "hotro@gamez.com.vn",
                        Phone = "02838373685",
                        FacebookUrl = "https://www.facebook.com/GameZ/",
                        Status = true
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.FooterLinks.Any())
            {
                await _context.FooterLinks.AddRangeAsync(
                    new FooterLink { Title = "Giới Thiệu", Url = "/Home/Introduction", GroupId = 1, DisplayOrder = 1, Status = true },
                    new FooterLink { Title = "Liên Hệ", Url = "/Home/Contact", GroupId = 1, DisplayOrder = 2, Status = true },
                    // Nhóm Tài Khoản (GroupId = 2)
                    new FooterLink { Title = "Tài Khoản Của Tôi", Url = "/Account/Index", GroupId = 2, DisplayOrder = 1, Status = true },
                    new FooterLink { Title = "Yêu Thích", Url = "/Account/Favorite", GroupId = 2, DisplayOrder = 2, Status = true },
                    new FooterLink { Title = "Lịch Sử Đơn Hàng", Url = "/Account/Order", GroupId = 2, DisplayOrder = 3, Status = true },
                    // Nhóm Danh Mục (GroupId = 3)
                    new FooterLink { Title = "Nintendo", Url = "/Product/Index?brands=nintendo", GroupId = 3, DisplayOrder = 1, Status = true },
                    new FooterLink { Title = "Xbox", Url = "/Product/Index?brands=xbox", GroupId = 3, DisplayOrder = 2, Status = true },
                    new FooterLink { Title = "PlayStation", Url = "/Product/Index?brands=playstation", GroupId = 3, DisplayOrder = 3, Status = true },
                    new FooterLink { Title = "Sega", Url = "/Product/Index?brands=sega", GroupId = 3, DisplayOrder = 4, Status = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Sliders.Any())
            {
                await _context.Sliders.AddRangeAsync
                (
                    new Slider { Title = "Đồng Hồ Citizen", Description = "Sản Phẩm Nổi Bật", Image = "/images/ricky-kharawala-Yka2yhGJwjc-unsplash 1.png", Link = "/Product/ProductDetail/2", DisplayOrder = 1, Status = true },
                    new Slider { Title = "Đồng Hồ Citizen-Eco", Description = "Giảm giá đến 15%", Image = "/images/Artboard-1.jpg", Link = "/Product/ProductDetail/5", DisplayOrder = 2, Status = true },
                    new Slider { Title = "Đồng Hồ Doxa", Description = "Biểu tượng của đẳng cấp và phong cách", Image = "/images/default-large.jpg", Link = "/Product/ProductDetail/9", DisplayOrder = 3, Status = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Abouts.Any())
            {
                await _context.Abouts.AddRangeAsync
                (
                    new About
                    {
                        Content = @"
                        GameZ không chỉ là nơi để mua sắm, mà còn là một thế giới để bạn khám phá, chinh phục và đắm chìm trong vũ trụ game console.
                        <br />
                        Với sứ mệnh ""Nơi An Tâm Trải Nghiệm Game Chính Hãng"", GameZ được xây dựng để mang đến cho cộng đồng game thủ những sản phẩm máy chơi game console hàng đầu, đảm bảo chất lượng và nguồn gốc xuất xứ. Chúng tôi cam kết cung cấp cho khách hàng những mẫu máy chơi game hoàn hảo về cả hiệu năng lẫn thiết kế, giúp bạn sẵn sàng cho mọi cuộc phiêu lưu.
                        <br />
                        Bên cạnh đó, GameZ hướng đến việc mang lại trải nghiệm mua sắm trực tuyến dễ dàng, an toàn và nhanh chóng. Thông qua hệ thống hỗ trợ thanh toán và vận hành vững mạnh, chúng tôi đảm bảo mọi giao dịch đều được thực hiện một cách thuận tiện và đáng tin cậy.
                        ",
                        Address = "458/3F Nguyễn Hữu Thọ, P. Tân Hưng, Quận 7, TP.HCM",
                        Phone = "0963303119",
                        Email = "2200009907@nttu.edu.vn"
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Policies.Any())
            {
                await _context.Policies.AddRangeAsync
                (
                    new Policy
                    {
                        Title = "Giao hàng nhanh",
                        Content = @"Chúng tôi cam kết cung cấp dịch vụ giao hàng nhanh chóng và đáng tin cậy. Đơn hàng của bạn sẽ được xử lý và giao trong vòng 1-2 ngày làm việc, tùy thuộc vào địa chỉ giao hàng. 
                                Đặc biệt, đối với các đơn hàng trong khu vực nội thành, chúng tôi sẽ giao trong ngày nếu đơn hàng được đặt trước 12h00. 
                                Mọi chi phí giao hàng sẽ được hiển thị rõ ràng khi bạn thanh toán, và miễn phí vận chuyển cho đơn hàng có giá trị từ [số tiền cụ thể] trở lên. 
                                Chúng tôi luôn nỗ lực mang đến trải nghiệm giao hàng nhanh chóng, tiện lợi và không gây phiền phức cho khách hàng."
                    },
                    new Policy
                    {
                        Title = "Miễn phí giao hàng",
                        Content = @"Cửa hàng sẽ miễn phí giao hàng cho tất cả các đơn hàng trong phạm vi nội thành.
                                Đối với các đơn hàng ở phạm vi ngoài thành phố thì sẽ được tính phí vận chuyển.
                                Thời gian nhận hàng sẽ từ 1-5 ngày tùy vào địa điểm nhận hàng.
                                Cửa hàng sẽ lựa chọn đối tác vận chuyển uy tín để đảm bảo đồng hồ được giao đến khách hàng một cách an toàn và đúng thời gian.
                                Trong quá trình vận chuyển, nếu sản phẩm bị hư hỏng hoặc thất lạc, cửa hàng sẽ chịu trách nhiệm hoàn toàn và có thể gửi lại sản phẩm mới hoặc hoàn tiền cho khách hàng.
                                Chính sách miễn phí giao hàng có thể không áp dụng cho các khu vực vùng sâu, vùng xa hoặc quốc tế, và trong trường hợp này, khách hàng sẽ được thông báo rõ ràng về các chi phí phát sinh."
                    },
                    new Policy
                    {
                        Title = "Cam kết chính hãng",
                        Content = @"Cửa hàng cam kết tất cả đồng hồ bán ra đều là hàng chính hãng, được nhập khẩu hoặc phân phối trực tiếp từ nhà sản xuất hoặc đại lý ủy quyền.
                                Mỗi sản phẩm sẽ đi kèm với các giấy tờ chứng nhận chính hãng, bao gồm sổ bảo hành, hóa đơn mua hàng, và các giấy tờ liên quan khác.
                                Đồng hồ mua tại cửa hàng sẽ được bảo hành theo tiêu chuẩn của nhà sản xuất. Thời gian bảo hành và các dịch vụ đi kèm sẽ được thực hiện tại các trung tâm bảo hành ủy quyền.
                                Nếu khách hàng chứng minh được sản phẩm là hàng giả, cửa hàng cam kết hoàn trả toàn bộ số tiền đã thanh toán và có thể bồi thường thêm tùy theo chính sách cụ thể.
                                Cửa hàng sẽ cung cấp dịch vụ hậu mãi, bao gồm sửa chữa và bảo trì đồng hồ, với cam kết sử dụng linh kiện chính hãng.
                                Cửa hàng có thể áp dụng chính sách đổi trả linh hoạt nếu khách hàng phát hiện sản phẩm có lỗi sản xuất hoặc không đúng với mô tả ban đầu."
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Roles.Any())
            {
                await _context.Roles.AddRangeAsync
                (
                    new Role { Type = "User" },
                    new Role { Type = "Admin" }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Accounts.Any())
            {
                await _context.Accounts.AddRangeAsync
                (
                    new Account { Username = "admin", Password = "admin", RoleId = 2, },
                    new Account { Username = "user1", Password = "user1", RoleId = 1 },
                    new Account { Username = "user2", Password = "user2", RoleId = 1, },
                    new Account { Username = "user3", Password = "user3", RoleId = 1, },
                    new Account { Username = "user4", Password = "user4", RoleId = 1 },
                    new Account { Username = "user5", Password = "user5", RoleId = 1, },
                    new Account { Username = "user6", Password = "user6", RoleId = 1 },
                    new Account { Username = "user7", Password = "user7", RoleId = 1, },
                    new Account { Username = "user8", Password = "user8", RoleId = 1 },
                    new Account { Username = "user9", Password = "user9", RoleId = 1, },
                    new Account { Username = "user10", Password = "user10", RoleId = 1 }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Customers.Any())
            {
                await _context.Customers.AddRangeAsync
                (
                    new Customer { FullName = "Nguyễn Văn A", Phone = "0123456789", Address = "123 Đường ABC, Quận 1", Email = "vana@gmail.com", Dob = DateOnly.ParseExact("1990-01-01", "yyyy-MM-dd"), Gender = true, AccountId = 2, DisplayName = "user1" },

                    new Customer { FullName = "Trần Thị B", Phone = "0987654321", Address = "456 Đường DEF, Quận 2", Email = "btran@gmail.com", Dob = DateOnly.ParseExact("1992-02-02", "yyyy-MM-dd"), Gender = false, AccountId = 3, DisplayName = "user2" },

                    new Customer { FullName = "Lê Văn C", Phone = "0123456780", Address = "789 Đường GHI, Quận 3", Email = "cle@gmail.com", Dob = DateOnly.ParseExact("1988-03-03", "yyyy-MM-dd"), Gender = true, AccountId = 4, DisplayName = "user3" },

                    new Customer { FullName = "Phạm Thị D", Phone = "0987654310", Address = "321 Đường JKL, Quận 4", Email = "dpham@gmail.com", Dob = DateOnly.ParseExact("1985-04-04", "yyyy-MM-dd"), Gender = false, AccountId = 5, DisplayName = "user4" },

                    new Customer { FullName = "Nguyễn Văn E", Phone = "0123456790", Address = "654 Đường MNO, Quận 5", Email = "evan@gmail.com", Dob = DateOnly.ParseExact("1995-05-05", "yyyy-MM-dd"), Gender = true, AccountId = 6, DisplayName = "user5" },

                    new Customer { FullName = "Trần Thị F", Phone = "0987654322", Address = "987 Đường PQR, Quận 6", Email = "ftran@gmail.com", Dob = DateOnly.ParseExact("1990-06-06", "yyyy-MM-dd"), Gender = false, AccountId = 7, DisplayName = "user6" },

                    new Customer { FullName = "Lê Văn G", Phone = "0123456781", Address = "135 Đường STU, Quận 7", Email = "gle@gmail.com", Dob = DateOnly.ParseExact("1982-07-07", "yyyy-MM-dd"), Gender = true, AccountId = 8, DisplayName = "user7" },

                    new Customer { FullName = "Phạm Thị H", Phone = "0987654311", Address = "246 Đường VWX, Quận 8", Email = "hpham@gmail.com", Dob = DateOnly.ParseExact("2000-07-07", "yyyy-MM-dd"), Gender = true, AccountId = 9, DisplayName = "user8" },

                    new Customer { FullName = "Nguyễn Văn I", Phone = "0123456791", Address = "357 Đường YZ, Quận 9", Email = "ivan@gmail.com", Dob = DateOnly.ParseExact("2002-08-30", "yyyy-MM-dd"), Gender = true, AccountId = 10, DisplayName = "user9" },

                    new Customer { FullName = "Trần Thị J", Phone = "0987654323", Address = "468 Đường ABCD, Quận 10", Email = "jtran@gmail.com", Dob = DateOnly.ParseExact("1996-01-11", "yyyy-MM-dd"), Gender = true, AccountId = 11, DisplayName = "user10" }
                );
                await _context.SaveChangesAsync();
            }
        }

    }
}