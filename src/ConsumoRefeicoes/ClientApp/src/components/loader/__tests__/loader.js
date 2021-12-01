import React from 'react';
import { render, screen } from '@testing-library/react';
import { LoaderContext } from '../../../lib/context/loader-context';
import Loader from '../index';

describe('Loader Component', () => {
  test('Render a loader', () => {
    render(
      <LoaderContext.Provider value>
        <Loader />
      </LoaderContext.Provider>,
    );

    expect(screen.getByTestId('loader')).toBeInTheDocument();
  });
});
