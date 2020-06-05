import React from 'react';
import { Header } from './Components/Header';
import { BrowserRouter, Route, Redirect, Switch } from 'react-router-dom';
import { PurchaseTransactionHome } from './PurchaseTransaction/PurchaseTransactionHome';
import { PurchaseTransactionPage } from './PurchaseTransaction/PurchaseTransactionPage';
import { StoreHome } from './StoreProfile/StoreHome';
import { StorePage } from './StoreProfile/StorePage';

function App() {
  return (
    <div className="App">
      <Header />
      <BrowserRouter>
        <Switch>
          <Redirect from="/home" to="/" />
          <Route exact path="/" component={PurchaseTransactionHome} />
          <Route
            path="/PurchaseTransaction/:id"
            component={PurchaseTransactionPage}
          />
          <Route path="/Store" component={StoreHome} />
          <Route path="/StoreInfo/:id" component={StorePage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
