import React, { useEffect, useState, useContext, useRef } from 'react';

import {
  Card,
  CardContent,
  CardActions,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Button,
  Fab,
  Snackbar,
} from '@material-ui/core';
import dayjs from 'dayjs';
import isBetween from 'dayjs/plugin/isBetween';
import timezone from 'dayjs/plugin/timezone';
import Autocomplete from '@material-ui/lab/Autocomplete';
import RemoveIcon from '@material-ui/icons/Remove';
import FingerprintIcon from '@material-ui/icons/Fingerprint';
import AddIcon from '@material-ui/icons/Add';
import { Alert, AlertTitle } from '@material-ui/lab';
import MuiAlert from '@material-ui/lab/Alert';
import { Box, BoxTitle } from '../../components/box-card';
import RefeicoesAPI from '../../lib/api/refeicoes';
import { LoaderContext } from '../../lib/context/loader-context';
import './pagina1.css';
import '../../assets/css/unimed.css';

const Home = () => {
  dayjs.extend(isBetween);
  dayjs.extend(timezone);

  const mes = dayjs().format('MM');
  const ano = dayjs().format('YYYY');
  const [checkMeal, setCheckMeal] = useState(false);
  const [titleButtom, setTitleButton] = useState('');
  const [codRef, setCodRef] = useState(0);
  const [dados, setDados] = useState([]);
  const [input, setInput] = useState('');
  const [success, setSuccess] = useState(false);
  const [confirm, setConfirm] = useState(false);
  const [message, setMessage] = useState('');
  const [typeMessage, setTypeMessage] = useState('');
  const [showFrigobar, setShowFrigobar] = useState(false);
  const [changeColor, setChangeColor] = useState(false);
  const [dataEmployee, setDataEmployee] = useState(null);
  const [itemsArray, setItemsArray] = useState([]);
  const [searchOptions, setSearchOptions] = useState([]);
  const [initialArray, setInitialArray] = useState([]);
  const [chooseArray, setChooseArray] = useState([]);
  const [selectItem, setSelectItem] = useState([]);
  const [corBotaoRefeicao, setCorBotaoRefeicao] = useState('btn-secondary');
  const [corBotao, setCorBotao] = useState('btn-secondary');
  const [acceptNotRegister, setAcceptNotRegister] = useState(false);
  const [acceptMealAgain, setAcceptMealAgain] = useState(false);
  const [others, setOthers] = useState(false);
  const [identifyType, setIdentifyType] = useState('');
  const { isLoading, setIsLoading } = useContext(LoaderContext);
  const vertical = 'top';
  const horizontal = 'center';

  const FormataHora = (valor) => {
    const hora = valor.substr(0, 2);
    let min = valor.substr(3, 4);
    const dh = hora * 60;
    min *= 1;
    return dh + min;
  };

  const onCleanMainButtons = () => {
    setCorBotaoRefeicao('btn-secondary');
    setCorBotao('btn-secondary');
    setChangeColor(false);
    setShowFrigobar(false);
  };

  const onCleanInformation = () => {
    setItemsArray(dados);
    setDataEmployee('');
    setInput('');
    setSelectItem([]);
    setConfirm(false);
    setChooseArray([]);
    onCleanMainButtons();
    setAcceptMealAgain(false);
    setAcceptNotRegister(false);
    setOthers(false);
    console.log(initialArray);
  };

  const Message = (props) => {
    return <MuiAlert elevation={6} variant="filled" {...props} />;
  };

  const onCleanFrigobar = async () => {
    const array = chooseArray;
    const result = [];
    array.forEach((v, i) => {
      if (v.usU_CODREF < 6) {
        result[i] = v;
      }
    });
    setOthers(false);
    setSelectItem([]);
    setChooseArray(result);
    setItemsArray(dados);
  };

  const searchEmployee = async (matricula) => {
    if (!matricula) {
      onCleanInformation();
    }

    setInput(matricula);
    const resp = await RefeicoesAPI.searchUser(matricula);

    if (resp?.data[0]?.situacao !== '1') {
      setDataEmployee(null);
      setSuccess(true);
      setTypeMessage('error');
      setMessage(
        'Não é possível realizar a operação: Colaborador Em situação de Férias ou Afastamento.',
      );
    } else if (resp.success === true) {
      // onCleanFrigobar();

      setChooseArray([]);
      onCleanMainButtons();
      setDataEmployee(resp?.data[0]);
      setItemsArray(dados);
      setOthers(false);
      setIdentifyType('D');
    }
  };

  const IdentifyMeal = (time) => {
    let codigo;
    let title;
    if (time >= 360 && time <= 660) {
      title = 'LANCHE MANHÃ';
      codigo = 1;
    } else if (time >= 661 && time <= 900) {
      title = 'ALMOÇO';
      codigo = 2;
    } else if (time >= 901 && time <= 1169) {
      title = 'LANCHE TARDE';
      codigo = 3;
    } else if (time >= 1170 && time <= 1349) {
      title = 'LANCHE NOITE';
      codigo = 4;
    } else if (time < 360 || time >= 1350) {
      title = 'JANTA';
      console.log(title);
      codigo = 5;
    }
    return codigo;
  };

  const GetMealById = async (codigo) => {
    const obj = {};
    let exists;
    obj.matricula = dataEmployee?.matricula;
    obj.competencia = dayjs().format('DD-MM-YYYY');
    const resp = await RefeicoesAPI.getMealById(obj);
    const retorno = resp?.data;
    retorno?.data.forEach((v) => {
      const codigoMeal = IdentifyMeal(v.usU_HORCON);
      if (codigoMeal === codigo) {
        exists = 1;
      }
    });
    return exists;
  };

  const GetRegister = async () => {
    const obj = {};
    obj.matricula = dataEmployee?.matricula;
    obj.competencia = dayjs().format('DD-MM-YYYY');
    const resp = await RefeicoesAPI.getRegister(obj);
    return resp;
  };
  /* eslint-disable consistent-return */
  const CatchDigital = async () => {
    onCleanInformation();
    const obj = {};
    obj.matricula = dataEmployee?.matricula;
    obj.competencia = dayjs().format('DD-MM-YYYY');
    setIsLoading(true);
    const resp = await RefeicoesAPI.findUser();

    if (resp.data.success) {
      setIdentifyType('C');
      setInput(resp.data.data.numcad);
      setDataEmployee({
        matricula: resp.data.data.numcad,
        name: resp.data.data.nomfun,
      });
      setIsLoading(false);
      // return resp;
    } else {
      setSuccess(true);
      setTypeMessage('error');
      setMessage(
        'Não foi possível identificar o colaborador(a)! Tente novamente.',
      );
      setIsLoading(false);
    }
  };

  const onSubmmit = async (e) => {
    if (e) {
      e.preventDefault();
    }
    const filtered = chooseArray.filter((el) => {
      return el;
    });
    const hora = FormataHora(dayjs().format('HH:mm'));
    const data = dayjs().format('DD/MM/YYYY');
    const post = {};
    post.USU_HORCON = hora;
    post.food = filtered;
    post.USU_DATCON = data;
    post.USU_NUMCAD = dataEmployee?.matricula;
    post.USU_TPCAPT = identifyType;

    const resp = await RefeicoesAPI.postFood(post);
    if (resp.data) {
      setSuccess(true);
      setTypeMessage('success');
      setMessage('Salvo com Sucesso!');
      onCleanFrigobar();
      onCleanInformation();
    } else {
      setTypeMessage('error');
      setMessage('Não há informações para serem salvas!');
    }
  };

  const onValidation = async (type) => {
    let meal;
    let codigoMeal;
    const register = await GetRegister();
    const filtered = chooseArray.filter((el) => {
      return el;
    });
    chooseArray.forEach((value) => {
      if (value.usU_CODREF < 6) {
        meal = true;
        codigoMeal = value.usU_CODREF;
      }
    });

    const result = await GetMealById(codigoMeal);
    if (type === 'ponto') {
      if (register.data === 0 && meal) {
        // setSuccess(true);
        setCheckMeal(true);
        setTypeMessage('error');
        setMessage({
          inicioMsg: 'O colaborador(a) ',
          colaborador: `${dataEmployee?.name} não`,
          negrito: ' marcações de PONTO',
          fimMsg: ' no dia de hoje!',
        });
        console.log(checkMeal);
        return;
      }
    }

    if (meal && result > 0) {
      // setCheckMeal(true);
      if (codigoMeal === 2 || codigoMeal === 5) {
        setAcceptMealAgain(false);
        setTypeMessage('error');
        setMessage(
          `O colaborador ${dataEmployee?.name} já possui lançamento para esta refeição!'`,
        );
        setSuccess(true);
        return;
      }
      setTypeMessage('info');
      setMessage({
        inicioMsg: 'O colaborador(a) ',
        colaborador: `${dataEmployee?.name} já`,
        negrito: ' possui lançamento',
        fimMsg: ' para esta refeição',
      });
      return;
      // setAcceptNotRegister(false);
    }
    if (filtered.length === 1 && meal) {
      onSubmmit();
    } else if (filtered.length > 0) {
      setConfirm(true);
    } else {
      setSuccess(true);
      setTypeMessage('error');
      setMessage('Não há informações para serem salvas!');
    }

    setAcceptMealAgain(false);
    setAcceptNotRegister(false);
  };

  const onConfirm = async (value) => {
    let meal;

    const register = await GetRegister();

    chooseArray.forEach((valor) => {
      if (valor.usU_CODREF < 6) {
        meal = true;
      } else if (valor.usU_CODREF > 5) {
        setOthers(true);
      }
    });

    if (!acceptNotRegister && register.data === 0) {
      onValidation('ponto');
      setAcceptNotRegister(value);
    } else {
      onValidation('meal');

      if (meal && !others && acceptMealAgain === true) {
        onSubmmit();
      } else {
        setAcceptMealAgain(value);
      }
    }
  };

  const handleClose = () => {
    setSuccess(false);
    setConfirm(false);
  };

  const onClickBotao = (value) => {
    setCorBotao(!value ? 'btn-success' : 'btn-secondary');
  };

  const onChangeFrigobar = (value) => {
    setShowFrigobar(!value);
    onCleanFrigobar();
    onClickBotao(value);
  };

  const onAllFood = async () => {
    let dateMes = Number(mes);
    let dateAno = Number(ano);

    // dateMes = type === 'previous' ? Number(mes) - 1 : Number(mes) + 1;
    if (dateMes < 1) {
      dateMes = 12;
      dateAno -= 1;
    }

    if (dateMes > 12) {
      dateMes = 1;
      dateAno += 1;
    }
    const resp = await RefeicoesAPI.getAllFood(
      dateAno + dateMes.toString().padStart(2, '0'),
    );
    const items = [];
    let ids = '';
    if (resp.success === true) {
      resp?.data.forEach((item, i) => {
        if (item.usU_CODREF > 5) {
          items[i] = item;
        }

        ids += ids === '' ? `${item.usU_CODREF}` : `,${item.usU_CODREF}`;
      });
      const resp2 = await RefeicoesAPI.foodUnused(ids.toString());
      const everyData = [...resp.data, ...resp2.data];
      const data = everyData;
      const everyItem = [...items, ...resp2.data];
      console.log(resp2);
      // const initial = JSON.parse(JSON.stringify(data));
      // everyData.concat(resp.data, resp2.data);
      setDados(data);
      setSearchOptions(everyItem);
      setItemsArray(everyData);

      // const initial = data.map((el) => {
      //   return el;
      // });
      setInitialArray(data);
    }
  };

  const onRemoveRefeicao = async (codigo) => {
    const array = chooseArray;
    const items = [];
    const teste = array.find((ch) => (ch ? ch.usU_CODREF < 5 : false));
    array.forEach((v, i) => {
      if (v.usU_CODREF !== codigo) {
        items[i] = v;
      }
      if (teste) {
        if (v.usU_CODREF === teste.usU_CODREF && v.usU_CODREF !== codigo) {
          items[i] = teste;
        }
      }
    });

    setChooseArray(items);
  };

  const onChange = (valor, type) => {
    const data = [];
    const novo = chooseArray;
    const items = [...dados];
    let remove;
    // let teste;
    items.map((value, i) => {
      const exist = novo.find((ch) =>
        ch ? ch.usU_CODREF === items[i].usU_CODREF : false,
      );

      valor.map((select) => {
        if (select.usU_CODREF === value.usU_CODREF) {
          data[i] = items[i];
          novo[i] = items[i];
          if (data[i].usU_QTDREF === 0 || !exist) {
            data[i].usU_QTDREF = 1;
          }
        }
        return data;
      });
      return data;
    });

    if (type === 'select-option') {
      novo?.map((v, i) => {
        // if (v.usU_CODREF > 5) {
        data[i] = v;
        // }
        return data;
      });
    }

    if (type === 'remove-option' && valor.length > 0) {
      // novo?.forEach(() => {

      remove = novo.filter((a) => !data.includes(a) && a.usU_CODREF > 5);
      // remove = novo.filter((ch, i) =>
      //   // let retorno;
      //   ch ? ch.usU_CODREF !== data[i].usU_CODREF && ch.usU_CODREF > 5 : false,
      // );
      // });

      const teste = novo.find((ch) => (ch ? ch.usU_CODREF < 5 : false));
      novo.forEach((v, i) => {
        if (v.usU_CODREF !== remove[0].usU_CODREF) {
          data[i] = v;
        }
        if (teste) {
          if (
            v.usU_CODREF === teste.usU_CODREF &&
            v.usU_CODREF !== remove[0].usU_CODREF
          ) {
            data[i] = teste;
          }
        }
      });
    }

    if (data.length > 0) {
      setItemsArray(data);
      setChooseArray(data);
      setSelectItem(data.filter((v) => v.usU_CODREF > 5));
    } else {
      setItemsArray(dados);
      setSelectItem(valor);
    }

    if (type === 'clear' || data.length === 0) {
      novo?.forEach((v, i) => {
        if (v.usU_CODREF < 6) {
          data[i] = v;
        }
      });
      setChooseArray(data);
    }
  };

  const onChangeQtd = (acao, index) => {
    const data = [...dados];
    const item = itemsArray;
    const choose = chooseArray;
    const exist = choose.find((ch) =>
      ch ? ch.usU_CODREF === data[index].usU_CODREF : false,
    );
    if (exist) {
      if (!choose[index].usU_QTDREF) {
        choose[index].usU_QTDREF = 0;
      }
      if (acao === 'less' && choose[index].usU_QTDREF > 0) {
        choose[index].usU_QTDREF -= 1;
      } else if (acao === 'plus') {
        choose[index].usU_QTDREF += 1;
      } else if (acao) {
        choose[index].usU_QTDREF = acao;
      } else {
        choose[index].usU_QTDREF = 0;
      }

      setChooseArray(choose);
    } else {
      data[index].usU_QTDREF = 0;

      if (acao === 'less' && data[index].usU_QTDREF > 0) {
        data[index].usU_QTDREF -= 1;
      } else if (acao === 'plus') {
        data[index].usU_QTDREF += 1;
      } else if (acao !== 'less' && acao !== 'plus') {
        data[index].usU_QTDREF = parseInt(acao, 10);
      } else {
        data[index].usU_QTDREF = 0;
      }
      choose[index] = data[index];
    }
    item[index] = data[index];

    if (choose[index].usU_QTDREF === 0) {
      onRemoveRefeicao(choose[index].usU_CODREF);
    }

    setDados(data);
    setItemsArray(item);
    setChooseArray(choose);
  };

  const onClickBotaoRefeicao = () => {
    // if (!value) {
    const array = chooseArray;
    const result = dados.map((v, i) => {
      const desc = v.descricao.toUpperCase();
      if (desc.trim() === titleButtom.trim()) {
        v.usU_QTDREF = 1;
        array[i] = v;
      }
      return array;
    });
    console.log(result);
    setChooseArray(array);
    // }
    // else {
    //   onRemoveRefeicao(codRef);
    // }
    // setChangeColor(!value);
    // setCorBotaoRefeicao(!value ? 'btn-success' : 'btn-secondary');
  };

  const onChangeBotaoRefeicao = (value) => {
    if (!value) {
      onClickBotaoRefeicao();
    } else {
      onRemoveRefeicao(codRef);
    }
    setChangeColor(!value);
    setCorBotaoRefeicao(!value ? 'btn-success' : 'btn-secondary');
  };

  const RenderHeader = () => {
    return (
      <thead>
        <tr key={1} className="bg-cinza-claro ml-5">
          {/* <th scope="col" className="mt-2" style={{ maxWidth: '50px' }}>
            Código
          </th> */}
          <th scope="col" style={{ maxWidth: '50px' }}>
            Item
          </th>
          <th scope="col size120" style={{ textAlign: 'right' }}>
            Quantidade
          </th>
        </tr>
      </thead>
    );
  };

  const RenderItems = () => {
    const list =
      itemsArray &&
      itemsArray.map((items, i) => {
        const statusColor = chooseArray[i]?.usU_QTDREF > 0 ? 'success' : '';
        const status = chooseArray[i]?.usU_QTDREF > 0 ? 'Selecionado' : '';
        if (items.usU_CODREF > 5) {
          return (
            <tr key={items.usU_CODREF}>
              {/* <th scope="row" style={{ maxWidth: '70px' }}>
                {items.usU_CODREF}
              </th> */}
              <td>
                {items.descricao}{' '}
                <span className={`text-right badge badge-${statusColor}`}>
                  {status}
                </span>
              </td>
              <td className="text-right" style={{ textAlign: 'right' }}>
                <Fab
                  size="small"
                  className="mr-2"
                  onClick={() => onChangeQtd('less', i)}>
                  <RemoveIcon />
                </Fab>
                <TextField
                  id="outlined-basic"
                  size="small"
                  style={{ width: '10%' }}
                  variant="outlined"
                  value={(chooseArray && chooseArray[i]?.usU_QTDREF) || 0}
                  onChange={(v) => onChangeQtd(v.target.value, i)}
                />
                <Fab
                  size="small"
                  className="ml-2"
                  onClick={() => onChangeQtd('plus', i)}>
                  <AddIcon />
                </Fab>
              </td>
            </tr>
          );
        }

        return null;
      });
    return (
      <>
        <tbody id="navbar-exemplo2">{list}</tbody>
      </>
    );
  };

  const RenderHeaderShoops = () => {
    return (
      <thead>
        <tr key={1} className="ml-5">
          <th scope="col" style={{ width: '300px' }}>
            Item
          </th>
          <th scope="col" style={{ textAlign: 'right !important' }}>
            Quantidade
          </th>
        </tr>
      </thead>
    );
  };
  const RenderShopps = () => {
    const list = chooseArray?.map((items) => {
      return (
        <tr key={items.usU_CODREF} style={{ marginBottom: '35px' }}>
          <td style={{ width: '100%' }}>{items.descricao}</td>
          <td style={{ textAlign: 'right', width: '45%' }}>
            <h5>
              <span className="cor-marrom-unimed">{items.usU_QTDREF}</span>
            </h5>
          </td>
        </tr>
      );
    });
    return (
      <div>
        <tbody id="navbar-exemplo2">{list}</tbody>
      </div>
    );
  };

  const searchItems = (descricao) => {
    const search = [];

    dados.filter((s, i) => {
      const name = s.descricao && s.descricao.toUpperCase();
      return name.includes(descricao.toUpperCase()) && (search[i] = s);
    });

    if (descricao) {
      setItemsArray(search);
    } else {
      setItemsArray(dados);
    }
  };

  const render = () => {
    return (
      <Box>
        <div className="row">
          <div
            className="col-md-12 cor-laranja"
            style={{ textAlign: 'center' }}>
            <TextField
              label="Colaborador"
              variant="outlined"
              id="outlined-basic"
              disabled={isLoading}
              value={input}
              style={{ right: '-31px' }}
              onChange={(v) => searchEmployee(v.target.value)}
            />
            <Button
              style={{ color: 'orange', top: '-15px', right: '-31px' }}
              onClick={() => CatchDigital()}>
              <FingerprintIcon
                fontSize="large"
                style={{ fontSize: '4.188rem' }}
              />
            </Button>
            {dataEmployee && (
              <h2 style={{ marginLeft: '25px' }}>{dataEmployee?.name}</h2>
            )}
          </div>
        </div>
        <BoxTitle />
        <div
          className="row justify-content-center mb-1"
          style={{ height: '120px' }}>
          <div
            className={`col-md-4  ${
              !dataEmployee?.matricula ? 'b-not-allowed' : ''
            }`}>
            <button
              disabled={!dataEmployee?.matricula}
              type="button"
              className={`btn btn-lg ${corBotaoRefeicao}  btn-block ${
                !dataEmployee?.matricula ? 'cor-cinza-unimed' : ''
              }`}
              style={{
                height: '95%',
                width: '100%',
                color: !dataEmployee?.matricula ? '#a0a0a0' : '',
              }}
              onClick={() => onChangeBotaoRefeicao(changeColor)}>
              <h1 style={{ fontSize: '150%' }}>{titleButtom}</h1>
            </button>
          </div>
          <div
            className={`col-md-4  ${
              !dataEmployee?.matricula ? 'b-not-allowed' : ''
            }`}>
            <button
              disabled={!dataEmployee?.matricula}
              type="button"
              className={`btn btn-lg  ${corBotao}  btn-block `}
              style={{
                height: '95%',
                width: '100%',
                color: !dataEmployee?.matricula ? '#a0a0a0' : '',
              }}
              onClick={() => onChangeFrigobar(showFrigobar)}>
              <h1 style={{ fontSize: '150%' }}>FRIGOBAR</h1>
            </button>
          </div>
        </div>
        <Card className="mb-4">
          <CardContent>
            {showFrigobar && (
              <>
                <div className="row justify-content-center">
                  <div className="col-md-10">
                    <Autocomplete
                      className="mb-3"
                      multiple
                      options={searchOptions || []}
                      getOptionLabel={(option) => {
                        if (typeof option === 'string') {
                          return option;
                        }
                        if (option.descricao) {
                          return option.descricao;
                        }
                        return option;
                      }}
                      loadingText="Carregando..."
                      noOptionsText="Nenhum registro encontrado"
                      onChange={(e, v, a) => {
                        console.log(a);
                        onChange(v, a);
                      }}
                      onInputChange={(e, v) => {
                        searchItems(v);
                      }}
                      fullWidth
                      value={selectItem}
                      renderInput={(params) => (
                        <TextField
                          {...params}
                          size="medium"
                          label="Pesquisar por"
                          variant="outlined"
                        />
                      )}
                    />
                  </div>
                </div>
                <div className="row justify-content-center">
                  <div className="col-md-10  table-wrapper-scroll-y my-custom-scrollbar">
                    <table className="table  table-bordered  table-striped">
                      {RenderHeader()}
                      {RenderItems()}
                    </table>
                  </div>
                </div>
              </>
            )}
            <div className="row mt-4 text-center">
              <div className="col-md-6 text-left">
                <button
                  type="button"
                  className="btn btn-danger  btn-lg w-50"
                  onClick={() => onCleanInformation()}>
                  Cancelar
                </button>
              </div>
              <div className="col-md-6 text-right">
                <button
                  type="button"
                  className="btn btn-success bg-verde-primario btn-lg w-50"
                  onClick={() => onConfirm(chooseArray.length > 0)}>
                  Salvar
                </button>
              </div>
            </div>
            <Dialog
              className="justify-content-left"
              open={confirm}
              fullWidth
              onClose={handleClose}
              aria-labelledby="customized-dialog-title">
              <DialogTitle>ITENS SELECIONADOS</DialogTitle>
              <DialogContent dividers>
                <Alert color="warning">
                  <AlertTitle>
                    {RenderHeaderShoops()}
                    {RenderShopps()}
                  </AlertTitle>
                </Alert>
              </DialogContent>
              <DialogActions>
                <div className="col-md-6 text-center w-50">
                  <button
                    type="button"
                    className="btn btn-danger  btn-lg w-50"
                    onClick={() => {
                      setConfirm(false);
                    }}>
                    Cancelar
                  </button>
                </div>
                <div className="col-md-6 text-center w-50">
                  <button
                    type="button"
                    className="submit btn btn-success bg-verde-primario btn-lg w-50"
                    onClick={() => onSubmmit()}>
                    Confirmar
                  </button>
                </div>
              </DialogActions>
            </Dialog>
            <Snackbar
              open={success}
              autoHideDuration={6000}
              onClose={handleClose}
              anchorOrigin={{ vertical, horizontal }}
              key={vertical + horizontal}>
              {typeof message !== 'object' ? (
                <Message severity={`${typeMessage}`}>{message}</Message>
              ) : (
                <Message color={`${typeMessage}`} severity={`${typeMessage}`}>
                  {message?.inicioMsg} <b>{message?.colaborador}</b> não possui
                  <b>{message?.negrito}</b>
                  {message?.fimMsg}
                </Message>
              )}
            </Snackbar>
            <Dialog
              open={acceptNotRegister}
              fullWidth
              onClose={handleClose}
              aria-labelledby="customized-dialog-title">
              <DialogContent dividers>
                <Alert color={typeMessage} severity={typeMessage}>
                  <AlertTitle>
                    {message?.inicioMsg} <b>{message?.colaborador}</b>
                    <b>{message?.negrito}</b>
                    {message?.fimMsg}
                  </AlertTitle>
                </Alert>
              </DialogContent>
              <DialogActions>
                <div className="col-md-6 text-center">
                  <button
                    type="button"
                    className="btn btn-danger  btn-lg w-75"
                    onClick={() => setAcceptNotRegister(false)}>
                    Cancelar
                  </button>
                </div>
                <div className="col-md-6 text-center">
                  <button
                    type="button"
                    className="submit btn btn-success bg-verde-primario btn-lg w-75"
                    onClick={() => {
                      setAcceptNotRegister(false);
                      onConfirm(true);
                      setConfirm(!!(acceptNotRegister && acceptMealAgain));
                    }}>
                    Deseja prosseguir?
                  </button>
                </div>
              </DialogActions>
            </Dialog>
            <Dialog
              open={acceptMealAgain}
              fullWidth
              onClose={handleClose}
              aria-labelledby="customized-dialog-title">
              <DialogContent dividers>
                <Alert color={typeMessage} severity={typeMessage}>
                  <AlertTitle>
                    {message?.inicioMsg} <b>{message?.colaborador}</b>
                    <b>{message?.negrito}</b>
                    {message?.fimMsg}
                  </AlertTitle>
                </Alert>
              </DialogContent>
              <DialogActions>
                <div className="col-md-6 text-center">
                  <button
                    type="button"
                    className="btn btn-danger  btn-lg w-75"
                    onClick={() => setAcceptMealAgain(false)}>
                    Cancelar
                  </button>
                </div>
                <div className="col-md-6 text-center">
                  <button
                    type="button"
                    className="submit btn btn-success bg-verde-primario btn-lg w-75"
                    onClick={() => {
                      if (others) {
                        setConfirm(true);
                      } else {
                        onSubmmit();
                      }
                      setAcceptMealAgain(false);
                    }}>
                    Deseja prosseguir?
                  </button>
                </div>
              </DialogActions>
            </Dialog>
          </CardContent>
          <CardActions />
        </Card>
      </Box>
    );
  };

  const useInterval = (callback, delay) => {
    const savedCallback = useRef();

    // Remember the latest callback.
    useEffect(() => {
      savedCallback.current = callback;
    }, [callback]);

    // Set up the interval.
    useEffect(() => {
      function tick() {
        savedCallback.current();
      }
      if (delay !== null) {
        const id = setInterval(tick, delay);
        return () => clearInterval(id);
      }
    }, [delay]);
  };

  const onChangeTitleMeal = () => {
    const hora = FormataHora(dayjs().format('HH:mm'));
    let title;
    let codigo;

    if (hora >= 360 && hora <= 660) {
      title = 'LANCHE MANHÃ';
      codigo = 1;
    } else if (hora >= 661 && hora <= 900) {
      title = 'ALMOÇO';
      codigo = 2;
    } else if (hora >= 901 && hora <= 1169) {
      title = 'LANCHE TARDE';
      codigo = 3;
    } else if (hora >= 1170 && hora <= 1349) {
      title = 'LANCHE NOITE';
      codigo = 4;
    } else if (hora < 360 || hora >= 1350) {
      title = 'JANTA';
      codigo = 5;
    }

    setTitleButton(title);
    setCodRef(codigo);
  };

  useEffect(() => {
    setIsLoading(true);
    onAllFood();
    setIsLoading(false);
  }, []);

  useInterval(() => {
    onChangeTitleMeal();
  }, 1000);

  return render();
};

export default Home;
