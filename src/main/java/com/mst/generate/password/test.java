package com.mst.generate.password;

import java.util.HashMap;
import java.util.Map;

public class test {

    static int[] nums = {2, 7, 11, 15};

    public static void main(String[] args) {

        nums = twoSum(nums, 9);
        // for (int i = 0; i < nums.length; i++) {
        System.out.println("[" + nums[0] + "," + nums[1] + "]");
        // }
    }

    public static int[] twoSum(int[] nums, int target) {
        int[] result = new int[0];// //;= new int[2];
        //Arrays.stream(nums).map()
        Map<Integer, Integer> map = new HashMap<>();
        int p = 0;
        //List<Map<Integer,Integer>>  mapArray = new ArrayList<>();
        for (int i = 0; i < nums.length; i++) {
            //for (int j = i + 1; j < nums.length; j++) {
            if (nums[i + 1] < nums.length - 1)
                if (nums[i] + nums[i + 1] == target) {
                    //   System.out.println("[" + nums[i] + "," + nums[j] + "]");
                    result = new int[]{i, i + 1};
                }
            // }
        }
        return result;

    }
}