import React from 'react';

import './form.css';

const Select = ({
  label,
  field,
  children,
  onChange,
  form: { touched, errors },
  ...props
}) => {
  const campoComErro = touched[field.name] && errors[field.name];
  const classError = campoComErro ? 'form-error' : '';

  return (
    <div className={`form-group ${classError}`}>
      <label htmlFor={field.name} className={campoComErro ? 'form-error' : ''}>
        {label}
      </label>
      <select
        className={`form-control ${classError}`}
        id={field.name}
        type="text"
        onChange={onChange}
        {...field}
        {...props}>
        {children}
      </select>
      {campoComErro ? (
        <div className="invalid-feedback">{errors[field.name]}</div>
      ) : null}
    </div>
  );
};

export default Select;
