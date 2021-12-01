import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import Rotas from './rotas';
import { LoaderProvider } from './lib/context/loader-context';
import { AuthProvider } from './lib/context/auth-context';
import Loader from './components/loader';

import './App.css';

const supportsHistory = 'pushState' in window.history;

function App() {
  return (
    <LoaderProvider>
      <AuthProvider>
        <Loader />
        <BrowserRouter
          basename={process.env.PUBLIC_URL}
          forceRefresh={!supportsHistory}>
          <Rotas />
        </BrowserRouter>
      </AuthProvider>
    </LoaderProvider>
  );
}

export default App;
