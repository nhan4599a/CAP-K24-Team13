axios.defaults.timeout = 20000;
axios.defaults.baseURL = 'http://ec2-52-207-214-39.compute-1.amazonaws.com:3000';

axios.interceptors.request.use(async config => {
    if (!config.url.startsWith('https://cap-k24-team13.herokuapp.com/')) {
        let accessToken = await getAccessToken();
        config.headers.Authorization = `Bearer ${accessToken}`
    }
    return Promise.resolve(config);
});

axios.interceptors.response.use(axiosResp => {
    if (axiosResp.request.responseURL.includes('backendapi')) {
        if (!axiosResp.data.isSuccessed) {
            return Promise.reject(axiosResp.data.message);
        }
        return Promise.resolve(axiosResp.data.resultObj);
    } else {
        if (axiosResp.data instanceof Blob)
            return Promise.resolve(axiosResp.data);
        let resp = axiosResp.data;
        if (resp.responseCode != 200) {
            return Promise.reject(resp.errorMessage);
        }
        return Promise.resolve(resp.data);
    }
}, error => Promise.reject(error));

const productEndpoint = '/products';
const categoryEndpoint = '/categories';
const cartEndpoint = '/cart'
const interfaceEndpoint = '/interfaces';
const checkoutEndpoint = '/checkout';
const ratingProductEndpoint = '/rating';
const orderEndpoint = '/orders';
const statisticEndpoint = '/statistic';
const reportEndpoint = '/reports';
const userEndpoint = '/users'

function findProducts(shopId, keyword, pageNumber, pageSize) {
    if (shopId === 0)
        shopId = '0';
    const acctualEndpoint = productEndpoint + (shopId ? `/shop/${shopId}/` : '/') + 'search';
    const params = {
        pageNumber: pageNumber,
        pageSize: pageSize || 5
    };
    if (keyword)
        params.keyword = keyword;
    return axios.get(acctualEndpoint, {
        params
    });
}

function getProductImageUrl(imageFileName) {
    return `${axios.defaults.baseURL}${productEndpoint}/images/${imageFileName}`;
}

function getProductImage(imageFileName) {
    return axios.get(getProductImageUrl(imageFileName), {
        responseType: 'blob'
    }).then(blob => {
        blob.name = imageFileName;
        return blob;
    });
}

function activateProduct(id, isActivateCommand) {
    return axios.delete(
        `${productEndpoint}/${id}?action=${isActivateCommand ? 1 : 0}`
    );
}

function addProduct(formData) {
    return axios.post(productEndpoint, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function editProduct(productId, formData) {
    return axios.put(`${productEndpoint}/${productId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function getAllCategories(shopId) {
    const actualEndpoint = categoryEndpoint + (shopId ? `/shop/${shopId}` : '');
    return axios.get(actualEndpoint).then(paginatedResponse => paginatedResponse.data);
}

function getCategories(shopId, pageNumber, pageSize) {
    const actualEndpoint = categoryEndpoint + (shopId ? `/shop/${shopId}` : '');
    return axios.get(actualEndpoint, {
        params: {
            pageNumber: pageNumber,
            pageSize: pageSize || 5
        }
    });
}

function getCategoryImageUrl(imageFileName) {
    return `${axios.defaults.baseURL}${categoryEndpoint}/images/${imageFileName}`;
}

function getCategoryImage(imageFileName) {
    return axios.get(`${getCategoryImageUrl(imageFileName)}?${Date.now() / 1000}`, {
        responseType: 'blob'
    }).then(blob => {
        blob.name = imageFileName;
        return blob;
    });
}

function activateCategory(activateCommand) {
    if (!activateCommand)
        throw new Error('activeCommand can not be null');
    if (!activateCommand.id)
        throw new Error('id can not be null');
    if (!activateCommand.isActivateCommand && !activateCommand.shouldBeCascade)
        throw new Error('Action does not supported');
    var action = activateCommand.isActivateCommand ? 1 : 0;
    var cascade = activateCommand.shouldBeCascade ? 1 : 0;
    return axios.delete(
        `${categoryEndpoint}/${activateCommand.id}?action=${action}&cascade=${cascade}`
    );
}

function addCategory(shopId, formData) {
    return axios.post(`${categoryEndpoint}/shop/${shopId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        }
    });
}

function editCategory(categoryId, formData) {
    return axios.put(`${categoryEndpoint}/${categoryId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function getShopInterface(shopId) {
    return axios.get(`${interfaceEndpoint}/${shopId}`);
}

function getShopInterfaceImageUrl(imageFileName) {
    return `${axios.defaults.baseURL}${interfaceEndpoint}/images/${imageFileName}`;
}

function getShopInterfaceImage(imageFileName) {
    return axios.get(getShopInterfaceImageUrl(imageFileName), {
        responseType: 'blob'
    }).then(blob => {
        blob.name = imageFileName;
        return blob;
    });
}

function addShopInterface(shopId, formData) {
    return axios.post(`${interfaceEndpoint}/${shopId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function editShopInterface(shopId, formData) {
    return axios.put(`${interfaceEndpoint}/${shopId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function addProductToCart(userId, productId, quantity) {
    let formData = new FormData();
    formData.append('userId', userId);
    formData.append('productId', productId);
    formData.append('quantity', quantity);
    return axios.post(cartEndpoint, formData);
}

function updateCartQuantity(userId, productId, quantity) {
    let formData = new FormData();
    formData.append('userId', userId);
    formData.append('productId', productId);
    formData.append('quantity', quantity);
    return axios.put(cartEndpoint, formData);
}

function removeProductInCart(userId, productId) {
    return axios.delete(`${cartEndpoint}/${userId}/${productId}`);
}

function checkOut(userId, productIdList, shippingName, shippingPhone, shippingAddress, orderNotes) {
    let formData = new FormData();
    formData.append('requestModel.userId', userId);
    formData.append('requestModel.productIds', productIdList);
    formData.append('requestModel.shippingName', shippingName);
    formData.append('requestModel.shippingPhone', shippingPhone);
    formData.append('requestModel.shippingAddress', shippingAddress);
    formData.append('requestModel.orderNotes', orderNotes);
    return axios.post(checkoutEndpoint, formData);
}

function ratingProduct(userId, productId, star, comment) {
    let formData = new FormData();
    formData.append('UserId', userId);
    formData.append('ProductId', productId);
    formData.append('Star', star);
    formData.append('Message', comment);
    return axios.post(ratingProductEndpoint, formData);
}

function changeOrderStatus(orderId, newStatus) {
    return axios.post(`${orderEndpoint}/${orderId}`, newStatus, {
        headers: {
            "Content-Type": "application/json"
        }
    });
}

function importQuantityProduct(productId, importedQuantity) {
    return axios.post(`${productEndpoint}/${productId}/import`, importedQuantity, {
        headers: {
            "Content-Type": "application/json"
        }
    });
}

function getStatisticOfShop(shopId, strategy, start, end) {
    return axios.get(`${statisticEndpoint}/shop/${shopId}/orders`, {
        params: {
            strategy,
            start,
            end
        }
    });
}

function getRecentOrdersOfShop(shopId) {
    return axios.get(`${orderEndpoint}/shop/${shopId}`);
}

function findInvoices(shopId, key, value, pageNumber, pageSize) {
    const acctualEndpoint = `${orderEndpoint}/shop/${shopId}/search`;
    const params = {
        pageNumber: pageNumber,
        pageSize: pageSize || 5
    };
    if (key && value) {
        params.key = encodeURIComponent(key);
        params.value = encodeURIComponent(value);
    }
    return axios.get(acctualEndpoint, {
        params
    });
}

function reportInvoice(invoiceId) {
    return axios.post(`${reportEndpoint}/${invoiceId}`);
}

function getReports(pageNumber, pageSize) {
    return axios.get(reportEndpoint, {
        params: {
            pageNumber: pageNumber,
            pageSize: pageSize
        }
    });
}

function getCustomers(pageNumber, pageSize) {
    return axios.get(userEndpoint, {
        params: {
            pageNumber: pageNumber,
            pageSize: pageSize,
            customer: true
        }
    });
}

function unbanUser(userId) {
    return axios.post(`${userEndpoint}/unban/${userId}`);
}

function banUser(userId, dayCount) {
    let formData = new FormData();
    if (dayCount) {
        formData.append('dayCount', dayCount);
    }
    return axios.post(`${userEndpoint}/ban/${userId}`, formData);
}

function getShopInformation(shopId) {
    return axios.get(`https://emallsolution-backendapi.herokuapp.com/api/shops/${shopId}`);
}

function getAccessToken() {
    return axios.get('https://cap-k24-team13.herokuapp.com/authentication/token');
}

function getUserId() {
    return axios.get('https://cap-k24-team13.herokuapp.com/authentication/id');
}

function getShopId() {
    return axios.get('https://cap-k24-team13.herokuapp.com/authentication/shop');
}