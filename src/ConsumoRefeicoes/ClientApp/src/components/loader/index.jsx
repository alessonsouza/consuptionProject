import React, { useContext } from 'react';

import { ClapSpinner } from 'react-spinners-kit';
import { LoaderContext } from '../../lib/context/loader-context';

const Loader = () => {
  const { isLoading } = useContext(LoaderContext);

  const style = {
    position: 'fixed',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    zIndex: 1000,
  };

  return (
    <div data-testid="loader" style={style}>
      <ClapSpinner size={40} frontColor="#410050" loading={isLoading} />
    </div>
  );
};

export default Loader;
