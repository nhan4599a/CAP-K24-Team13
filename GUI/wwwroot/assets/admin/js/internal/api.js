axios.defaults.baseURL = 'https://localhost:44302/api/';
axios.defaults.timeout = 20000;

axios.interceptors.response.use(axiosResp => {
    if (axiosResp.data instanceof Blob)
        return Promise.resolve(axiosResp.data);
    var resp = axiosResp.data;
    if (resp.responseCode != 200) {
        return Promise.reject(resp.errorMessage);
    }
    return Promise.resolve(resp.data);
}, error => Promise.reject(error));

const productEndpoint = 'products';
const categoryEndpoint = 'categories';

function findProducts(keyword, pageNumber, pageSize, successCallback) {
    if (keyword === null || keyword === '')
        axios.get(productEndpoint, {
            params: {
                'paginationInfo.pageNumber': pageNumber,
                'paginationInfo.pageSize': pageSize
            }
        }).then(successCallback);
    else
        axios.get(productEndpoint, {
            params: {
                keyword: encodeURIComponent(keyword),
                'paginationInfo.pageNumber': pageNumber,
                'paginationInfo.pageSize:': pageSize || 5
            }
        }).then(successCallback);
}

function getProductImageUrl(imageFileName) {
    return `${axios.defaults.baseURL}${productEndpoint}/images/${imageFileName}`;
}

function getProductImage(imageFileName, callback) {
    axios.get(getProductImageUrl(imageFileName), {
        responseType: 'blob'
    }).then(blob => {
        blob.name = imageFileName;
        return blob;
    }).then(callback);
}

function deleteProduct(id, successCallback, errorCallback) {
    axios.delete(productEndpoint + `/${id}?action=0`).then(successCallback).catch(errorCallback);
}

function activeProduct(id, successCallback, errorCallback) {
    axios.delete(productEndpoint + `/${id}?action=1`).then(successCallback).catch(errorCallback);
}

function addProduct(formData, successCallback, errorCallback) {
    axios.post(productEndpoint, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(successCallback).catch(errorCallback);
}

function editProduct(id, formData, successCallback, errorCallback) {
    axios.put(productEndpoint + `/${id}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(successCallback).catch(errorCallback);
}

function getAllCategories(successCallback) {
    axios.get(categoryEndpoint).then(paginatedResponse => successCallback(paginatedResponse.data));
}

function getCategories(pageNumber, pageSize, successCallback) {
    axios.get(categoryEndpoint, {
        params: {
            pageNumber: pageNumber,
            pageSize: pageSize || 5
        }
    }).then(successCallback);
}

function deleteCategory(id, successCallback, errorCallback) {
    axios.delete(categoryEndpoint + `/${id}?action=0`).then(successCallback).catch(errorCallback);
}

function activeCategory(id, successCallback, errorCallback) {
    axios.delete(categoryEndpoint + `/${id}?action=1`).then(successCallback).catch(errorCallback);
}

function addCategory(category, successCallback, errorCallback) {
    axios.post(categoryEndpoint, category).then(successCallback).catch(errorCallback);
}

function editCategory(id, category, successCallback, errorCallback) {
    axios.put(categoryEndpoint + `/${id}`, category).then(successCallback).catch(errorCallback);
}