axios.defaults.timeout = 20000;
axios.defaults.baseURL = 'http://ec2-52-207-214-39.compute-1.amazonaws.com:3000';

axios.interceptors.response.use(axiosResp => {
    console.log(axiosResp);
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

async function activateProduct(id, isActivateCommand) {
    let accessToken = await getAccessToken();
    return axios.delete(
        `${productEndpoint}/${id}?action=${isActivateCommand ? 1 : 0}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        });
}

async function addProduct(formData) {
    let accessToken = await getAccessToken();
    return axios.post(productEndpoint, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function editProduct(id, formData) {
    let accessToken = await getAccessToken();
    return axios.put(productEndpoint + `/${id}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
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

async function activateCategory(activateCommand) {
    let accessToken = await getAccessToken();
    if (!activateCommand)
        throw new Error('activeCommand can not be null');
    if (!activateCommand.id)
        throw new Error('id can not be null');
    if (!activateCommand.isActivateCommand && !activateCommand.shouldBeCascade)
        throw new Error('Action does not supported');
    var action = activateCommand.isActivateCommand ? 1 : 0;
    var cascade = activateCommand.shouldBeCascade ? 1 : 0;
    return axios.delete(
        `${categoryEndpoint}/${activateCommand.id}?action=${action}&cascade=${cascade}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`
            }
        });
}

async function addCategory(formData) {
    let accessToken = await getAccessToken();
    return axios.post(categoryEndpoint, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function editCategory(id, formData) {
    let accessToken = await getAccessToken();
    return axios.put(categoryEndpoint + `/${id}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
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

async function addShopInterface(shopId, formData) {
    let accessToken = await getAccessToken();
    return axios.post(`${interfaceEndpoint}/${shopId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function editShopInterface(shopId, formData) {
    let accessToken = await getAccessToken();
    return axios.put(`${interfaceEndpoint}/${shopId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function addProductToCart(userId, productId, quantity) {
    let accessToken = await getAccessToken();
    let formData = new FormData();
    formData.append('userId', userId);
    formData.append('productId', productId);
    formData.append('quantity', quantity);
    return axios.post(`${cartEndpoint}`, formData, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function updateCartQuantity(userId, productId, quantity) {
    let accessToken = await getAccessToken();
    let formData = new FormData();
    formData.append('userId', userId);
    formData.append('productId', productId);
    formData.append('quantity', quantity);
    return axios.put(`${cartEndpoint}`, formData, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function removeProductInCart(userId, productId) {
    let accessToken = await getAccessToken();
    return axios.delete(`${cartEndpoint}/${userId}/${productId}`, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function checkOut(userId, productIdList, shippingName, shippingPhone, shippingAddress, orderNotes) {
    let accessToken = await getAccessToken();
    let formData = new FormData();
    formData.append('requestModel.userId', userId);
    formData.append('requestModel.productIds', productIdList);
    formData.append('requestModel.shippingName', shippingName);
    formData.append('requestModel.shippingPhone', shippingPhone);
    formData.append('requestModel.shippingAddress', shippingAddress);
    formData.append('requestModel.orderNotes', orderNotes);
    return axios.post(checkoutEndpoint, formData, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function ratingProduct(userId, productId, star, comment) {
    let accessToken = await getAccessToken();
    let formData = new FormData();
    formData.append('UserId', userId);
    formData.append('ProductId', productId);
    formData.append('Star', star);
    formData.append('Message', comment);
    return axios.post(ratingProductEndpoint, formData, {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });
}

async function changeOrderStatus(orderId, newStatus) {
    let accessToken = await getAccessToken();
    return axios.post(`${orderEndpoint}/${orderId}`, newStatus, {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`
        }
    });
}

function getAccessToken() {
    return axios.get('https://cap-k24-team13.herokuapp.com/token');
}