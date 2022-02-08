docker run -d -p 8500:8500 -p 8600:8600/udp --name=consul consul agent -server -ui -bootstrap-expect=1 -node=server-1 -client=0.0.0.0
wt dotnet run --project %~dp0/GUI
wt dotnet run --project %~dp0/Infrastructure/ApiGateway
wt dotnet run --project %~dp0/Service/ShopProductService
wt dotnet run --project %~dp0/Service/ShopInterfaceService
wt dotnet run --project %~dp0/Service/RatingService 
wt dotnet run --project %~dp0/Service/CheckoutService
wt dotnet run --project %~dp0/Service/OrderHistoryService
wt dotnet run --project %~dp0/AuthServer