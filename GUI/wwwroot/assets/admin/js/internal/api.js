axios.defaults.baseURL = 'https://localhost:44302/api/'

function findProducts(keyword, pageNumber, pageSize, callback) {
    axios.get('products', {
        params: {
            keyword: encodeURIComponent(keyword),
            'paginationInfo.pageNumber': pageNumber,
            'paginationInfo.pageSize:': pageSize || 5
        }
    }).then(resp => callback(resp.data));
}