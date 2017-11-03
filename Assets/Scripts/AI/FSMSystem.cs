using System.Collections.Generic;
using UnityEngine;

public class FSMSystem {
    private readonly List<FSMState> states;
    private FSMState currentState;
    public FSMState CurrentState {
        get {
            return currentState;
        }
    }

    public FSMSystem() {
        states = new List<FSMState>();
    }

    #region ����
    public void AddState(FSMState s) {
        // Check for Null reference before deleting
        if (s == null) {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0) {
            states.Add(s);
            currentState = s;
            currentState.DoBeforeEntering(null);
            return;
        }

        if (states.Contains(s)) {
            Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
            return;
        }

        states.Add(s);
    }
    #endregion

    #region ɾ��
    public void DeleteState(StateID id) {
        // Check for NullState before deleting
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState tempS = GetStateByID(id);
        if (tempS != null) {
            states.Remove(tempS);
            return;
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }
    #endregion

    #region �޸�
    //ǿ�ƽ���һ��״̬�����жϵ�ǰ״̬�Ƿ��ת������״̬
    public void ForceState(StateID id, BaseActor target) {
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }

        //�˳���ǰ״̬
        currentState.DoBeforeLeaving(target);

        //������ǰ״̬
        currentState = state;

        //������״̬
        currentState.DoBeforeEntering(target);
    }

    //ִ��״̬ת��
    public void PerformTransition(StateID id, BaseActor target) {
        // Check for NullTransition before changing the current state
        if (id == StateID.NullStateID) {
            Debug.LogError(" FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }
        //��ǰ״̬�Ƿ�����ƶ�ת��
        if (!currentState.IsHaveTransition(id)) {
            return;
        }
        //����״̬ID ��ȡ״̬
        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }
        
        //�˳���ǰ״̬
        currentState.DoBeforeLeaving(target);

        //������ǰ״̬
        currentState = state;

        //������״̬
        currentState.DoBeforeEntering(target);
    }
    #endregion

    #region �鿴
    //�Ƿ���ĳһ״̬��
    public bool IsInState(StateID id) {
        if (id == StateID.NullStateID) {
            return false;
        }

        return CurrentState.ID == id;
    }

    private FSMState GetStateByID(StateID id) {
        int count = states.Count;
        for (int i = 0; i < count; ++i) {
            if (states[i].ID == id) {
                return states[i];
            }
        }

        return null;
    }
    #endregion
}
