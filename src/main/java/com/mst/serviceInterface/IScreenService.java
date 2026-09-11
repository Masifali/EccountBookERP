package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.Screen;

public interface IScreenService {
    List<Screen> getAll();
    default Screen getById(Integer id) { return null; }
}
