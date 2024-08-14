using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Constants;
using Infrastructure.Identity;
using Domain.Enums;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context, UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!await _context.Images.AnyAsync())
        {
            var images = new List<ImageEntity>()
            {
                new ImageEntity
                {
                    Name = "background-1.jpg",
                    Title = "Dọc sông Venice mộng mơ tại Grand World",
                    Alt = "Những căn hộ đầy màu sắc dọc theo sông Venice mộng mơ tại Grand World",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Name = "background-2.png",
                    Title = "Biệt thự tại Mikazuki Japanese Resorts & Spa",
                    Alt = "Căn biệt thự có hồ bơi tại Mikazuki Japanese Resorts & Spa",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Name = "background-3.jpg",
                    Title = "Một góc bên sông Sài Gòn với những tòa nhà cao chọc trời",
                    Alt = "Một góc bên sông Sài Gòn với những tòa nhà cao chọc trời",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Name = "background-4.jpg",
                    Title = "Pullman Đà Nẵng Beach Resort",
                    Alt = "Pullman Đà Nẵng Beach Resort",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Name = "background-5.jpg",
                    Title = "Khu vực lễ tân tại Mikazuki Japanese Resorts & Spa",
                    Alt = "Khu vực lễ tân sang trọng tại Mikazuki Japanese Resorts & Spa",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Nhà phố biển thương mại Queen Pearl Marina Complex",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh nhà phố biển thương mại Queen Pearl Marina Complex",
                    Name = "queen_pearl.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Khu đô thị thương mại dịch vụ E.City Tân Đức",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh khu đô thị thương mại dịch vụ E.City Tân Đức",
                    Name = "e_city_tan_duc.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Tổ hợp phố thương mại cao cấp The SHOLI",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh tổ hợp phố thương mại cao cấp The SHOLI",
                    Name = "the_sholi.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Khu biệt thự compound cao cấp Sol Villas",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh khu biệt thự compound cao cấp Sol Villas",
                    Name = "sol_villas.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Tổ hợp du lịch - nghỉ dưỡng - giải trí - thể thao biển Thanh Long Bay",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh tổ hợp du lịch - nghỉ dưỡng - giải trí - thể thao biển Thanh Long Bay",
                    Name = "thanh_long_bay.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án biệt thự cap cấp Zenna Villas",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án biệt thự cap cấp Zenna Villas",
                    Name = "zenna_villas.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án căn hộ - du lịch biển Cadia Quy Nhơn",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án căn hộ - du lịch biển Cadia Quy Nhơn",
                    Name = "cadia_quy_nhon.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án Waterpoint - Riverside Community",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án Waterpoint - Riverside Community",
                    Name = "waterpoint.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án Hiệp Phước Harbour View",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án Hiệp Phước Harbour View",
                    Name = "hiep_phuong_harbour.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Phức hợp Đô thị Thương mại Dịch vụ & Du lịch biển Lagi New City",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh phức hợp Đô thị Thương mại Dịch vụ & Du lịch biển Lagi New City",
                    Name = "lagi_new_city.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án căn hộ chất lượng cao cấp Citigrand",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án căn hộ chất lượng cao cấp Citigrand",
                    Name = "citigrand.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Tổ hợp thương mại, dịch vụ, khách sạn và nhà ở cao cấp Eco Green Saigon",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh tổ hợp thương mại, dịch vụ, khách sạn và nhà ở cao cấp Eco Green Saigon",
                    Name = "eco_green_saigon.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Phức Hợp Thương Mại & Căn Hộ Cao Cấp Astral City",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh phức Hợp Thương Mại & Căn Hộ Cao Cấp Astral City",
                    Name = "astra_city.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
                new ImageEntity
                {
                    Title = "Dự án Terra Rosa Bình Chánh",
                    Type = (int)ImageTypeEnum.OnlyFull,
                    Alt = "Hình ảnh dự án Terra Rosa Bình Chánh",
                    Name = "terra_rosa_binh_chanh.jpg",
                    CreatedBy = 1,
                    UpdatedBy = 1
                },
            };
            _context.Images.AddRange(images);
            await _context.SaveChangesAsync();

            if (!await _context.Contents.AnyAsync())
            {
                var content = new ContentEntity
                {
                    Name = "Nội dung trang web mặc định",
                    HomeImage = images[0],
                    BgHomeImage = images[2],
                    NewsImage = images[3],
                    ContactImage = images[4],
                    Status = StatusConstant.Active,
                };
                for (var i = 0; i < 5; i++)
                {
                    content.ProjectSlider.Add(new SliderImageEntity
                    {
                        Image = images[i],
                        Order = i,
                    });
                }
                content.IntroduceSection = "<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
                    "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, " +
                    "when an unknown printer took a galley of type and scrambled it to make a type specimen book. " +
                    "It has survived not only five centuries, but also the leap into electronic typesetting, " +
                    "remaining essentially unchanged.</p>" +
                    "<p>It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, " +
                    "and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>";
                content.NewsMarketSection = "Đăng các bản tin thị trường, các bài viết thống kê, phân tích và dự đoán thị trường.";
                content.NewsProjectSection = "Cập nhật tin về các dự án sớm nhất để nhà đầu tư có thêm nhiều thông tin cần thiết.";

                _context.Contents.Add(content);
            }

            var html = "<p>Dự án Nhà ở xã hội Golden Square Lào Cai đã được UBND tỉnh Lào Cai\r\n                chấp thuận liên danh Công ty Cổ phần Đông Á Golden Square và Công ty Cổ phần\r\n                Foodinco Quy Nhơn là nhà đầu tư thực hiện, với tổng mức đầu tư hơn 2.085 tỷ đồng.\r\n                Đây là dự án nhằm từng bước cụ thể hóa Chương trình, Kế hoạch phát triển nhà ở trên\r\n                địa bàn tỉnh, góp phần cung cấp sản phẩm nhà ở cho người dân, đồng thời đảm bảo chỗ\r\n                ở ổn định cho các đối tượng được hưởng chính sách về nhà ở xã hội.</p>\r\n                <img title=\"Phối cảnh tổng thể dự án\" src=\"/upload/images/background-1.jpg\">\r\n                <p class=\"text-center fst-italic\">Phối cảnh tổng thể dự án</p>\r\n                <p>Tọa lạc tại ngã ba giao lộ Mỏ Sinh và Quang Thái, Golden Square Lào Cai sở hữu vị trí đắc địa ngay khu \r\n                trung tâm hành chính mới của thành phố. Dự án thừa hưởng hệ thống giao thông hoàn chỉnh, bài bản giúp kết \r\n                nối nhanh chóng tới những điểm đến quan trọng trong khu vực. Dự kiến khi hoàn thiện, Golden Square Lào Cai \r\n                sẽ cung cấp khoảng 2.192 căn hộ mang phong cách thiết kế hiện đại, thân thiện môi trường, đề cao tính bền \r\n                vững và tối ưu công năng, sẵn sàng cho sức chứa hơn 5.500 người. Bên cạnh đó, cư dân dự án còn được thừa \r\n                hưởng chuỗi tiện ích nội khu đồng bộ, cảnh quan xanh trong lành với bốn mặt thoáng đãng, mang đến cuộc sống \r\n                đa trải nghiệm, hiện đại và hạnh phúc hơn. Đặc biệt, các chủ sở hữu tương lai có thể yên tâm khi Golden Square Lào Cai \r\n                sở hữu pháp lý minh bạch, sổ hồng lâu dài và cam kết hỗ trợ thủ tục hoàn thiện hồ sơ nhanh gọn, thuận tiện từ Chủ đầu tư.</p>\r\n                <img title=\"Đại diện Chủ đầu tư và các đơn vị đối tác thực hiện nghi thức động thổ dự án\" src=\"/upload/images/background-3.jpg\">\r\n                <p class=\"text-center fst-italic\">Đại diện Chủ đầu tư và các đơn vị đối tác thực hiện nghi thức động thổ dự án</p>\r\n                <p>Tham dự buổi lễ có đại diện Chủ đầu tư - Công ty Cổ phần Đông Á Golden Square cùng đại diện đơn vị \r\n                Tư vấn thiết kế - Liên danh Công ty Cổ phần Tư vấn Thiết Kế Salvador Perez Arroyo &amp; cộng sự và Công ty Cổ phần A79, \r\n                đại diện đơn vị Tổng thầu thi công - Tập đoàn Xây dựng Delta Việt Nam và Công Ty Cổ phần Đầu tư Xây dựng và Nền móng Hà Nội, \r\n                các lãnh đạo Ban Quản lý dự án tỉnh Lào Cai và các Tư vấn giám sát.</p>\r\n                <p>Tại chương trình, ông Đỗ Đặng Dũng – Tổng Giám đốc Công ty Cổ phần Đông Á Golden Square chia sẻ:\r\n                <span class=\"fst-italic\"> “Với năng lực, kinh nghiệm về đầu tư – xây dựng các dự án nhà ở, khu đô thị, khách sạn cao cấp, \r\n                Chủ đầu tư Công ty Cổ phần Đông Á Golden Square cũng đã lên kế hoạch kỹ lưỡng đối với Nhà ở xã hội Golden Square Lào Cai. \r\n                Dự án được chuẩn bị bài bản từ khâu lên phương án thiết kế, lựa chọn đơn vị tư vấn, nhà thầu thi công có năng lực… nhằm \r\n                xây dựng nên một dự án hoàn chỉnh, đảm bảo chất lượng, đúng tiến độ và kiểm soát tốt chi phí.”</span></p>\r\n                <img title=\"Phối cảnh tiện ích nội khu dự án\" src=\"/upload/images/the_sholi.jpg\">\r\n                <p class=\"text-center fst-italic\">Phối cảnh tiện ích nội khu dự án</p>\r\n                <p><span class=\"fst-italic\">“Sau khi hoàn thiện, Nhà ở xã hội Golden Square Lào Cai tự tin mang \r\n                đến thị trường những căn hộ hiện đại tiêu chuẩn, chuỗi tiện ích nội khu đồng bộ cùng mức giá \r\n                phù hợp với người mua nhà là những cán bộ nhân viên đang công tác tại thành phố Lào Cai và các \r\n                khu vực phụ cận”</span> – ông Dũng cho biết.</p>";

            // Default roles
            var administratorRole = new Role(RoleConstant.Admin);

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
                await _roleManager.CreateAsync(new Role(RoleConstant.NewsPoster));
                await _roleManager.CreateAsync(new Role(RoleConstant.ProjectPoster));
                await _roleManager.CreateAsync(new Role(RoleConstant.Caller));
                await _roleManager.CreateAsync(new Role(RoleConstant.RegisteredEmailChecker));
                await _roleManager.CreateAsync(new Role(RoleConstant.ImagePoster));
                await _roleManager.CreateAsync(new Role(RoleConstant.ContentEditor));
                await _roleManager.CreateAsync(new Role(RoleConstant.Newbie));
            }

            // Default users
            var administrator = new User
            {
                UserName = "admin",
                Email = "phantrungtin01@gmail.com",
                Name = "Phan Trung Tín",
                Gender = (int)GenderEnum.Male,
                DateOfBirth = new DateTime(2001, 5, 11)
            };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Admin!123");
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                }
            }
            
            // Default cofiguration
            if (!await _context.Configurations.AnyAsync())
            {
                await _context.Configurations.AddAsync(new ConfigurationEntity
                {
                    SendEmail = "pptin105011@gmail.com",
                    SendEmailPassword = "pqrlnapsxvlwdhyd",
                    ReceiveEmail = "phantrungtin01@gmail.com",
                    ContactPhone = "0868754872",
                    ContactZaloPhone = "0868754872",
                    SystemPhone = "0868754872",
                    SystemEmail = "phantrungtin01@gmail.com",
                    SystemZaloLink = "https://zalo.me/0868754872",
                    SystemYoutubeLink = "https://www.youtube.com/@tinphantrung8394",
                    SystemFacebookLink = "https://www.facebook.com/tin.phan.1105",
                });
            }

            // Default news
            if (!await _context.News.AnyAsync())
            {
                List<NewsEntity> news = new List<NewsEntity> {
                    new NewsEntity
                    {
                        Ud = "terra-rosa-binh-chanh",
                        Name = "Dự án Terra Rosa Bình Chánh",
                        Type = NewsTypeConstant.Project,
                        Description = "Dự án Terra Rosa thuộc lô 13E, khu dân cư Phong Phú, mặt tiền Nguyễn Văn Linh. Vị trí thuận lợi, gắn kết phát triển hạ tầng kỹ thuật - xã hội, dễ dàng di chuyển về các quận trung tâm.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 19,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "astra-city-binh-duong",
                        Name = "Phức Hợp Thương Mại & Căn Hộ Cao Cấp Astral City",
                        Type = NewsTypeConstant.Project,
                        Description = "Astral City tọa lạc tại mặt tiền Quốc lộ 13, phường Bình Hòa, thành phố Thuận An, tỉnh Bình Dương. Đây là vị trí trung tâm của khu vực Bình Dương, kết nối thuận tiện với các khu vực xung quanh như TP.HCM, Đồng Nai, Bà Rịa - Vũng Tàu.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 18,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "eco-green-saigon",
                        Name = "Tổ hợp thương mại, dịch vụ, khách sạn và nhà ở cao cấp Eco Green Saigon",
                        Type = NewsTypeConstant.Project,
                        Description = "Dự án căn hộ Eco Green Quận 7 là Khu Phức Hợp đẳng cấp 5 sao quy mô cực lớn 14,36 hecta bao gồm 7 Block căn hộ, 1 tòa Văn phòng – Khách sạn cao cấp Park Hyatt - Tòa tháp cao nhất Quận 7 với chiều cao 68 Tầng.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 17,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "citigrand-tp-hcm",
                        Name = "Dự án căn hộ chất lượng cao cấp Citigrand",
                        Type = NewsTypeConstant.Project,
                        Description = "CITIGRAND là dự án căn hộ chất lượng cao cấp với vườn trên mái thời thượng, 6 lõi không gian xanh xuyên suốt, hồ bơi vô cực, vườn kết nối hạnh phúc, khu vui chơi trẻ em, khu BBQ, thẻ từ an ninh, sảnh đón sang trọng...",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 16,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "lagi-new-city-binh-thuan",
                        Name = "Phức hợp Đô thị Thương mại Dịch vụ & Du lịch biển Lagi New City",
                        Type = NewsTypeConstant.Project,
                        Description = "Lagi New City Là dự án Khu đô thi phức hợp nghĩ dưỡng tại Tâm điểm nghỉ dưỡng và du lịch biển Bình Thuận –  Lagi New City toả sáng như một viên ngọc giữa hòn biển đông xanh ngát, nơi núi rường và biển hoà vào nhau.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 15,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "hiep-phuoc-harbour-view-long-an",
                        Name = "Dự án Hiệp Phước Harbour View",
                        Type = NewsTypeConstant.Project,
                        Description = "Hiệp Phước Harbour View sở hữu vị trí “Độc Nhất Vô Nhi” tọa lạc trên 2 mặt tiền “View Hướng Sông – Mặt Hướng Lộ”. Hiệp Phước Harour View nằm ngay tại mặt tiền Đường Nguyễn Văn Tạo, xã Phước Vĩnh Đông, huyện Cần Giuộc, tỉnh Long An.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 14,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "waterpoint-tp-hcm",
                        Name = "Dự án Waterpoint - Riverside Community",
                        Type = NewsTypeConstant.Project,
                        Description = "“Thành phố bên sông” Waterpoint 355ha được biết đến là khu đô thị tích hợp hàng đầu tại cửa ngõ Tây Nam TP.HCM sở hữu địa thế phong thủy thịnh vượng trên bãi bồi được bao bọc bởi 5,8km sông Vàm Cỏ Đông.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 13,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "cadia-quy-nhon",
                        Name = "Dự án căn hộ - du lịch biển Cadia Quy Nhơn",
                        Type = NewsTypeConstant.Project,
                        Description = "Cadia Quy Nhơn là tổ hợp căn hộ hạng sang theo mô hình smart compound tại Quy Nhơn. Dự án sở hữu thiết kế sang trọng, tiện ích 5 sao và ứng dụng công nghệ 4.0 có một không hai vào thiết kế căn hộ.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 12,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "zenna-villas-vung-tau",
                        Name = "Dự án biệt thự cap cấp Zenna Villas",
                        Type = NewsTypeConstant.Project,
                        Description = "Zenna Villas là dự án biệt thự nghỉ dưỡng tuyệt vời, chỉ cách TP.HCM 1h30 phút đi xe. Dự án Zenna Villas mang đến cho khách hàng một không gian bình yên, giá trị của đẳng cấp thành đạt.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 11,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "thanh-long-bay-binh-thuan",
                        Name = "Tổ hợp du lịch - nghỉ dưỡng - giải trí - thể thao biển Thanh Long Bay",
                        Type = NewsTypeConstant.Project,
                        Description = "",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 10,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "sol-villas-tp-hcm",
                        Name = "Khu biệt thự compound cao cấp Sol Villas",
                        Type = NewsTypeConstant.Project,
                        Description = "Thanh Long Bay là tổ hợp đô thị Du lịch, Nghỉ dưỡng và Thể thao biển tọa lạc tại Mũi Kê Gà - Hàm Thuận Nam, Bình Thuận. Với quy mô 90,3 hecta nằm trên cung đường biển Quốc Gia, sở hữu hơn 2km bờ biển riêng.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 9,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "the-sholi-tp-hcm",
                        Name = "Tổ hợp phố thương mại cao cấp The SHOLI",
                        Type = NewsTypeConstant.Project,
                        Description = "The Sholi là dự án nhà phố thương mại với quy mô 1.45ha tại 311 An Dương Vương, P.An Lạc, Q.Bình Tân, Tp.HCM. Nhà phố The Sholi Bình Tân với diện tích đất từ 61-80m2 xây dựng 1 trệt, 1 lửng, 3 lầu & 1 sân thượng.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 8,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "e-city-tan-duc-long-an",
                        Name = "Khu đô thị thương mại dịch vụ E.City Tân Đức",
                        Type = NewsTypeConstant.Project,
                        Description = "Khu đô thị E-City Tân Đức thuộc Quần thể Công nghiệp – Dịch vụ – Đô thị Tân Đức được thiết kế và quản lý theo mô hình kiến trúc hiện đại của Hoa Kỳ được thực hiện bởi nhà thiết kế xây dựng Morris Architecture.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 7,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new NewsEntity
                    {
                        Ud = "queen-pearl-binh-thuan",
                        Name = "Nhà phố biển thương mại Queen Pearl Marina Complex",
                        Type = NewsTypeConstant.Project,
                        Description = "Queen Pearl Marina Complex sở hữu vị trí đắc địa, kết nối giao thông thuận lợi cùng thiết kế cảnh quan cây xanh đẹp mắt đem tới không gia sống lý tưởng giá trị nghỉ dưỡng cao cấp.",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 6,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                };

                await _context.News.AddRangeAsync(news);
            }
            
            // Default projects
            if (!await _context.Projects.AnyAsync())
            {

                List<ProjectEntity> projects = new List<ProjectEntity> {
                    new ProjectEntity
                    {
                        Ud = "terra-rosa-binh-chanh",
                        Name = "Dự án Terra Rosa Bình Chánh",
                        Type = ProjectTypeConstant.Apartment,
                        Address = "TP. Hồ Chí Minh",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 19,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "astra-city-binh-duong",
                        Name = "Phức Hợp Thương Mại & Căn Hộ Cao Cấp Astral City",
                        Type = ProjectTypeConstant.Apartment,
                        Address = "Bình Dương",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 18,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "eco-green-saigon",
                        Name = "Tổ hợp thương mại, dịch vụ, khách sạn và nhà ở cao cấp Eco Green Saigon",
                        Type = ProjectTypeConstant.Apartment,
                        Address = "TP. Hồ Chí Minh",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 17,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "citigrand-tp-hcm",
                        Name = "Dự án căn hộ chất lượng cao cấp Citigrand",
                        Type = ProjectTypeConstant.Apartment,
                        Address = "TP. Hồ Chí Minh",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 16,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "lagi-new-city-binh-thuan",
                        Name = "Phức hợp Đô thị Thương mại Dịch vụ & Du lịch biển Lagi New City",
                        Type = ProjectTypeConstant.Ground,
                        Address = "Bình Thuận",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 15,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "hiep-phuoc-harbour-view-long-an",
                        Name = "Dự án Hiệp Phước Harbour View",
                        Type = ProjectTypeConstant.Ground,
                        Address = "Long An",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 14,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "waterpoint-tp-hcm",
                        Name = "Dự án Waterpoint - Riverside Community",
                        Type = ProjectTypeConstant.Ground,
                        Address = "TP. Hồ Chí Minh",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 13,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "cadia-quy-nhon",
                        Name = "Dự án căn hộ - du lịch biển Cadia Quy Nhơn",
                        Type = ProjectTypeConstant.ResortRealEstate,
                        Address = "Quy Nhơn",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 12,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "zenna-villas-vung-tau",
                        Name = "Dự án biệt thự cap cấp Zenna Villas",
                        Type = ProjectTypeConstant.ResortRealEstate,
                        Address = "Vũng Tàu",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 11,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "thanh-long-bay-binh-thuan",
                        Name = "Tổ hợp du lịch - nghỉ dưỡng - giải trí - thể thao biển Thanh Long Bay",
                        Type = ProjectTypeConstant.ResortRealEstate,
                        Address = "Bình Thuận",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 10,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "sol-villas-tp-hcm",
                        Name = "Khu biệt thự compound cao cấp Sol Villas",
                        Type = ProjectTypeConstant.Villa,
                        Address = "TP. Hồ Chí Minh",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 9,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "the-sholi-tp-hcm",
                        Name = "Tổ hợp phố thương mại cao cấp The SHOLI",
                        Type = ProjectTypeConstant.Villa,
                        Address = "TP. Hồ Chí Minh",
                        State = "implementing",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 8,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "e-city-tan-duc-long-an",
                        Name = "Khu đô thị thương mại dịch vụ E.City Tân Đức",
                        Type = ProjectTypeConstant.Villa,
                        Address = "Long An",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = false,
                        ImageId = 7,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                    new ProjectEntity
                    {
                        Ud = "queen-pearl-binh-thuan",
                        Name = "Nhà phố biển thương mại Queen Pearl Marina Complex",
                        Type = ProjectTypeConstant.Villa,
                        Address = "Bình Thuận",
                        State = "implemented",
                        Content = html,
                        Status = StatusConstant.Active,
                        IsHighlight = true,
                        ImageId = 6,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    },
                };

                await _context.Projects.AddRangeAsync(projects);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
