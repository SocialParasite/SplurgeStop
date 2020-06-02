import React, { FC, useState, Fragment, useEffect, ChangeEvent } from 'react';
import { Page } from './../Page';
import { RouteComponentProps } from 'react-router-dom';
import { DetailedStoreData, getStore } from './StoreData';

interface RouteParams {
  id: string;
}

export const StorePage: FC<RouteComponentProps<RouteParams>> = ({ match }) => {
  const [store, setStore] = useState<DetailedStoreData | null>(null);

  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const doGetStore = async (id: string) => {
      const foundStore = await getStore(id);
      setStore(foundStore);
    };

    if (match.params.id) {
      const storeId = match.params.id;
      doGetStore(storeId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (store != null) {
      store.name = e.currentTarget.value;
    }
    setStore(store);
  };

  return (
    <Page>
      <button onClick={editModeClick}>Edit</button>
      <div>
        {store !== null && (
          <Fragment>
            <div>
              {isEditing ? (
                <form>
                  <div>
                    <input
                      type="text"
                      name="name"
                      value={store.name}
                      onChange={handleChange}
                    ></input>
                  </div>
                  <button type="submit">Save</button>
                </form>
              ) : (
                <div>
                  <h1>{store.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
};
