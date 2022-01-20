﻿using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class ShopInterfaceRepository : IShopInterfaceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly List<ShopInterfaceDTO> FakeInterfaces = new List<ShopInterfaceDTO>
        {
            new ShopInterfaceDTO
            {
                ShopId = 1,
                ShopName = "Future World",
                ShopAddress = " 234 Nguyễn Thị Minh Khai, Phường 6, Quận 3, Thành phố Hồ Chí Minh",
                ShopEmail = "Futureworld@gmail.com",
                ShopPhoneNumber = "090 383 85 40",
                Images = new string[]
                {
                    "https://futureworld.com.vn/media/promobanners//c/c/ccm1-facade.jpg"
                }
            },
            new ShopInterfaceDTO
            {
                ShopId = 2,
                ShopName = "Apple Store",
                ShopAddress = " 133 Đường 3 Tháng 2, Phường 11, Quận 10, Thành phố Hồ Chí Minh",
                ShopEmail = "AppleStore@gmail.com",
                ShopPhoneNumber = "090 383 85 40",
                Images = new string[]
                {
                    "data:image/jpeg;base64,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"
                }
            },
             new ShopInterfaceDTO
            {
                ShopId = 3,
                ShopName = "Nike",
                ShopAddress = " Số 8 Điện Biên Phủ, Đa Kao, Quận 1, Thành phố Hồ Chí Minh",
                ShopEmail = "Nike@gmail.com",
                ShopPhoneNumber = "090 383 85 40",
                Images = new string[]
                {
                    "data:image/jpeg;base64,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"
                }
            },
             new ShopInterfaceDTO
            {
                ShopId = 4,
                ShopName = "Bitis",
                ShopAddress = " 193 Lê Quang Định, Phường 7, Bình Thạnh, Thành phố Hồ Chí Minh",
                ShopEmail = "Bitis@gmail.com",
                ShopPhoneNumber = "090 383 85 40",
                Images = new string[]
                {
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5oyJQiR5SbQrk9QmPuMiDQZPItOir5ATMkQ&usqp=CAU"
                }
            },
             new ShopInterfaceDTO
            {
                ShopId = 5,
                ShopName = "Converse",
                ShopAddress = " 234 Nguyễn Thị Minh Khai, Phường 6, Quận 3, Thành phố Hồ Chí Minh",
                ShopEmail = "Converse@gmail.com",
                ShopPhoneNumber = "090 383 85 40",
                Images = new string[]
                {
                    "data:image/jpeg;base64,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EBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQP//Z"
                }
            },

        };

        public ShopInterfaceRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> FindShopInterfaceByShopIdAsync(int shopId)
        {
            if (shopId != 0)
                return CommandResponse<ShopInterfaceDTO>.Success(
                    FakeInterfaces.FirstOrDefault(@interface => @interface.ShopId == shopId));
            var shopInterface = await _dbContext.ShopInterfaces
                .AsNoTracking().FirstOrDefaultAsync(e => e.ShopId == shopId);
            return CommandResponse<ShopInterfaceDTO>.Success(_mapper.MapToShopInterfaceDTO(shopInterface));
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> EditShopInterfaceAsync(int shopId,
            CreateOrEditInterfaceRequestModel requestModel)
        {
            var shopInterface = await _dbContext.ShopInterfaces.FirstOrDefaultAsync(e => e.ShopId == shopId);
            if (shopInterface == null)
            {
                shopInterface = new ShopInterface().AssignByRequestModel(requestModel);
                _dbContext.ShopInterfaces.Add(shopInterface);
            }
            else
            {
                shopInterface.AssignByRequestModel(requestModel);
                _dbContext.Entry(shopInterface).State = EntityState.Modified;
            }
            try
            {
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                return CommandResponse<ShopInterfaceDTO>.Error(ex.Message, ex);
            }
            return CommandResponse<ShopInterfaceDTO>.Success(_mapper.MapToShopInterfaceDTO(shopInterface));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}