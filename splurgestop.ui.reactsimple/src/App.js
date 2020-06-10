import React from 'react';
import { Header } from './Components/Header';
import { BrowserRouter, Route, Redirect, Switch } from 'react-router-dom';
import { PurchaseTransactionList } from './PurchaseTransaction/PurchaseTransactionList';
import { PurchaseTransactionPage } from './PurchaseTransaction/PurchaseTransactionPage';
import { StoreList } from './StoreProfile/StoreList';
import { StorePage } from './StoreProfile/StorePage';
import { NewStore } from './StoreProfile/NewStore';
import { NewPurchaseTransaction } from './PurchaseTransaction/NewPurchaseTransaction';

function App() {
  return (
    <div className="App">
      <Header />
      <BrowserRouter>
        <Switch>
          <Redirect from="/home" to="/" />
          <Route exact path="/" component={PurchaseTransactionList} />
          <Route
            exact
            path="/PurchaseTransaction/Add"
            component={NewPurchaseTransaction}
          />
          <Route
            exact
            path="/PurchaseTransaction/:id"
            component={PurchaseTransactionPage}
          />
          <Route exact path="/Store" component={StoreList} />
          <Route exact path="/Store/Add" component={NewStore} />
          <Route path="/StoreInfo/:id" component={StorePage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
