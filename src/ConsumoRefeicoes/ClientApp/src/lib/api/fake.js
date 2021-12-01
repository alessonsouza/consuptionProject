const FakeAPI = {
  submitAjax: (values) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        console.log(values);
        resolve();
      }, 1000);
    });
  },
};

export default FakeAPI;
