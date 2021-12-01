import { Formik, Field, Form } from 'formik';
import React from 'react';
import * as Yup from 'yup';

import { Input } from '../../components/form';

import './form-login.css';

const validationSchema = Yup.object().shape({
  username: Yup.string().required('Favor informar seu login da rede'),
  password: Yup.string().required('Favor informar sua senha'),
});

const LoginForm = ({ loginData, submitForm }) => {
  return (
    <>
      <Formik
        initialValues={loginData}
        onSubmit={submitForm}
        validationSchema={validationSchema}>
        {(formProps) => (
          <div>
            <Form>
              <div className="row">
                <div className="col-md-12 col-xs-12">
                  <Field
                    label="Usuário"
                    id="usuario"
                    name="username"
                    placeholder="Usuário"
                    value={formProps.values.username}
                    component={Input}
                  />
                </div>

                <div className="col-md-12 col-xs-12">
                  <Field
                    label="Senha"
                    placeholder="Senha"
                    id="senha"
                    name="password"
                    type="password"
                    value={formProps.values.password}
                    component={Input}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-12 mt-1" style={{ textAlign: 'center' }}>
                  <button
                    type="submit"
                    className="btn bg-verde-primario cor-branco btn-block">
                    Conectar
                  </button>
                </div>
              </div>
            </Form>
          </div>
        )}
      </Formik>
    </>
  );
};

export default LoginForm;
