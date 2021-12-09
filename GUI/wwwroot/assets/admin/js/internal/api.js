axios.defaults.baseURL = 'https://localhost:44302/api/';
axios.defaults.timeout = 20000;

axios.interceptors.response.use(axiosResp => {
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

function deleteProduct(id, successCallback, errorCallback) {
    axios.delete(productEndpoint + `/${id}`).then(successCallback).catch(errorCallback);
}

function addProduct(product, successCallback, errorCallback) {
    axios.post(productEndpoint, {
        data: JSON.stringify(product)
    }).then(successCallback).catch(errorCallback);
}

function editProduct(id, product, successCallback) {
    axios.put(productEndpoint + `/${id}`, {
        data: JSON.stringify(product)
    }).then(successCallback);
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
    axios.delete(categoryEndpoint + `/${id}`).then(successCallback).catch(errorCallback);
}

function addCategory(category, successCallback, errorCallback) {
    axios.post(categoryEndpoint, {
        data: JSON.stringify(category)
    }).then(successCallback).catch(errorCallback);
}

function editCategory(id, category, successCallback) {
    axios.put(categoryEndpoint + `/${id}`, {
        data: JSON.stringify(category)
    }).then(successCallback);
}