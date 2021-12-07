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

function findProducts(keyword, pageNumber, pageSize, callback) {
    axios.get(productEndpoint, {
        params: {
            keyword: encodeURIComponent(keyword),
            'paginationInfo.pageNumber': pageNumber,
            'paginationInfo.pageSize:': pageSize || 5
        }
    }).then(callback);
}

function deleteProduct(id, callback) {
    axios.delete(productEndpoint + `/${id}`).then(callback);
}