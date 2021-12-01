import api from './api';

const RefeicoesAPI = {
  searchItem: async (values) => {
    const returnFromApi = await api.get(`/refeicoes/search-item/${values}`);
    return returnFromApi.data;
  },
  searchUser: async (values) => {
    const returnFromApi = await api.get(`/refeicoes/search-user/${values}`);
    return returnFromApi.data;
  },
  getAllFood: async (values) => {
    const returnFromApi = await api.get(`/refeicoes/get-all-food/${values}`);
    return returnFromApi.data;
  },
  postFood: async (values) => {
    console.log('VALUES');
    console.log(values);
    let returnFromApi;
    await api
      .post(`/refeicoes/submit`, {
        food: values.food,
        USU_HORCON: values.USU_HORCON,
        USU_DATCON: values.USU_DATCON,
        USU_NUMCAD: values.USU_NUMCAD,
        USU_TPCAPT: values.USU_TPCAPT
      })
      .then((res) => {
        returnFromApi = res.data;
        console.log(res);
      });
    return returnFromApi;
  },
  getMealById: async (values) => {
    const returnFromApi = await api.get(
      `/refeicoes/get-meal-by-id/${values.matricula}/${values.competencia}`,
    );

    return returnFromApi;
  },
  getRegister: async (values) => {
    const returnFromApi = await api.get(
      `/refeicoes/get-register/${values.matricula}/${values.competencia}`,
    );

    return returnFromApi;
  },
  capturar: async () => {
    const returnFromApi = await api.get(`/refeicoes/get-capturar`);

    return returnFromApi?.data;
  },
  comparar: async (values) => {
    let returnFromApi;
    await api
      .post(`/refeicoes/get-comparar`, {
        DESDIGAUX: values,
      })
      .then((res) => {
        returnFromApi = res;
      });
    return returnFromApi;
  },
  findUser: async () => {
    const returnFromApi = await api.get('/refeicoes/find-user');

    return returnFromApi;
  },
  foodUnused: async (values) => {
    const returnFromApi = await api.get(`/refeicoes/get-food-unused/${values}`);
    return returnFromApi.data;
  },
};

export default RefeicoesAPI;
