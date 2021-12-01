import React from 'react';
import PropTypes from 'prop-types';

import './form.css';

const Input = ({ field, form: { touched, errors }, ...props }) => {
  const { label, name } = props;

  const campoComErro = touched[field.name] && errors[field.name];
  const classError = campoComErro ? 'form-error' : '';

  return (
    <div className="form-group">
      <label htmlFor={name} className={campoComErro ? 'form-error' : ''}>
        {label}
      </label>
      <input
        className={`form-control ${classError}`}
        name={field.name}
        {...field}
        {...props}
      />

      {campoComErro ? (
        <div className="invalid-feedback">{errors[field.name]}</div>
      ) : null}
    </div>
  );
};

Input.propTypes = {
  label: PropTypes.string.isRequired,
};

export default Input;
