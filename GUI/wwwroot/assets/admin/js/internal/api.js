axios.defaults.timeout = 20000;
axios.defaults.baseURL = 'https://localhost:3000';

axios.interceptors.response.use(axiosResp => {
    if (axiosResp.data instanceof Blob)
        return Promise.resolve(axiosResp.data);
    let resp = axiosResp.data;
    if (resp.responseCode != 200) {
        return Promise.reject(resp.errorMessage);
    }
    return Promise.resolve(resp.data);
}, error => Promise.reject(error));

const productEndpoint = '/products';
const categoryEndpoint = '/categories';
const cartEndpoint = '/cart'
const interfaceEndpoint = '/interfaces';
const checkoutEndpoint = '/checkout';
const ratingProductEndpoint = '/rating';
const orderEndpoint = '/orders'

function findProducts(keyword, pageNumber, pageSize) {
    if (keyword === null || keyword === '')
        return axios.get(productEndpoint + '/search', {
            params: {
                'paginationInfo.pageNumber': pageNumber,
                'paginationInfo.pageSize': pageSize
            }
        });
    else
        return axios.get(productEndpoint + '/search', {
            params: {
                keyword: encodeURIComponent(keyword),
                'paginationInfo.pageNumber': pageNumber,
                'paginationInfo.pageSize:': pageSize || 5
            }
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

function editProduct(id, formData) {
    return axios.put(productEndpoint + `/${id}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function getAllCategories() {
    return axios.get(categoryEndpoint).then(paginatedResponse => paginatedResponse.data);
}

function getCategories(pageNumber, pageSize) {
    return axios.get(categoryEndpoint, {
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

function addCategory(formData) {
    return axios.post(categoryEndpoint, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}

function editCategory(id, formData) {
    return axios.put(categoryEndpoint + `/${id}`, formData, {
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
    return axios.post(`${cartEndpoint}`, formData);
}

function updateCartQuantity(userId, productId, quantity) {
    let formData = new FormData();
    formData.append('userId', userId);
    formData.append('productId', productId);
    formData.append('quantity', quantity);
    return axios.put(`${cartEndpoint}`, formData);
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
    let formData = new FormData();
    formData.append('invoiceId', orderId);
    formData.append('newStatus', newStatus);
    return axios.post(orderEndpoint, formData);
}