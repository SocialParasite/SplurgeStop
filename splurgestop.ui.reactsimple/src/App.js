import React from 'react';
import { Header } from './Components/Header';
import { BrowserRouter, Route, Redirect, Switch } from 'react-router-dom';
import { PurchaseTransactionList } from './PurchaseTransaction/PurchaseTransactionList';
import { PurchaseTransactionPage } from './PurchaseTransaction/PurchaseTransactionPage';
import { StoreList } from './StoreProfile/StoreList';
import { StorePage } from './StoreProfile/StorePage';

function App() {
  return (
    <div className="App">
      <Header />
      <BrowserRouter>
        <Switch>
          <Redirect from="/home" to="/" />
          <Route exact path="/" component={PurchaseTransactionList} />
          <Route
            path="/PurchaseTransaction/:id"
            component={PurchaseTransactionPage}
          />
          <Route path="/Store" component={StoreList} />
          <Route path="/StoreInfo/:id" component={StorePage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
